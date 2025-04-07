import amqp from 'amqplib'
let interval_check_conn = undefined
let isInit = true
let connection
let channel
let isConnecting = false
export async function rabbitPublish (
  queue,
  host = 'amqp://localhost:2093',
  message
) {
  let connection
  try {
    connection = await amqp.connect(host)
    channel = await connection.createChannel()

    await channel.assertQueue(queue, { durable: false })
    channel.sendToQueue(queue, Buffer.from(JSON.stringify(message)))
    console.log(" [x] Sent '%s'", message)
    await channel.close()
  } catch (err) {
    console.warn(err)
  } finally {
    if (connection) await connection.close()
  }
}

async function connectBroker (
  host = 'amqp://localhost:2093',
  isCheckConnect = true
) {
  if (!isInit && connection) return { connection, channel }
  try {
    console.log('Start init rabbit')
    connection = await amqp.connect(host + '?heartbeat=60')
    channel = await connection.createChannel()
    connection.on('close', e => {
      isInit = true
      connection = undefined
      console.log('close rabbit', e)
    })
    connection.on('error', e => {
      isInit = true
      connection = undefined
      console.log('error rabbit', e)
    })
  } catch (error) {
    throw error
  } finally {
  }

  return { connection, channel }
}
export async function rabbitConsume (
  queue,
  host = 'amqp://localhost:2093',
  callback
) {
  if (!connection) {
    await connectBroker(host)
    await rabbitConsume(queue, host, callback)
    return
  }
  if (!isInit) return
  try {
    console.log('Start consume:', connection, channel)
    // await channel.assertQueue(queue, { durable: false })
    await channel.consume(
      queue,
      async message => {
        if (message) {
          try {
            console.log(
              " [i]Rabbit Received '%s'",
              JSON.parse(message.content.toString())
            )

            callback(message.content.toString())
          } catch (error) {
            console.log(" [x]Rabbit Received Error:'%s'", error)
          } finally {
            await channel.ackAll()
          }
        }
      },
      { noAck: true }
    )

    console.log(' [*] Waiting for messages. To exit press CTRL+C')
  } catch (err) {
    console.warn(err)
  } finally {
    if (interval_check_conn == undefined) {
      interval_check_conn = setInterval(async () => {
        await rabbitConsume(queue, host, callback)
      }, process.env.CHECK_RABBIT_CONN_INTERVAL || 2000)
      isInit = false
    }
  }
}
