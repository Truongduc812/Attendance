import { createServer } from 'http'
import { Server } from 'socket.io'
import { rabbitPublish, rabbitConsume } from './rabbit.js'
import dotenv from 'dotenv'
dotenv.config()

const CONST_PORT = process.env.HOST_PORT

const httpServer = createServer()
const io = new Server(httpServer, {
  // cors: {
  //   origin: "*"
  // },
  path: '/socket.io/',
  connectTimeout: 6000,
  transports: ['websocket', 'polling', 'webtransport']
})

let dicClientId2Name = {}
let dicRoom = {}
function getUsersInRoom (room) {
  const clients = io.sockets.adapter.rooms.get(room)
  if (clients) {
    return Array.from(clients).map(clientId => dicClientId2Name[clientId])
  }
  return []
}
function getActiveRooms () {
  const activeRooms = []
  for (var key in dicRoom) {
    activeRooms.push(key)
  }
  return activeRooms
}

io.on('connection', async client => {
  console.log('[client connected...]', client.id)

  client.on('input', (room, senderName, message) => {
    console.log(`\r\n[${room}]: [${senderName}] ${message}`)
    client.to(room).emit(room, message)
  })

  client.on('join_room', (room, name) => {
    console.log(`\r\n[join_room]: [${room}]: [${name}]`)
    client.join(room)
    dicClientId2Name[client.id] = name
    dicRoom[room] = true
  })

  client.on('leave_room', room => {
    const name = dicClientId2Name[client.id]
    console.log(`\r\n[leave_room]: [${room}]: [${name}]`)
    client.leave(room)
    delete dicClientId2Name[client.id]
  })

  client.on('ack', room => {
    const name = dicClientId2Name[client.id]
    console.log(`ack: [${room}]: [${name}]`)
  })

  client.on('disconnecting', () => {
    const name = dicClientId2Name[client.id]
    console.log(`[disconnecting] [${name}]`, client.rooms)
  })

  client.on('disconnect', () => {
    //console.log('client disconnect...', client.id);
    delete dicClientId2Name[client.id]
  })

  client.on('error', err => {
    console.log('error from client:', client.id, err)
  })
})

io.of('/').adapter.on('create-room', room => {
  console.log(`-- room ${room} was created`)
})

io.of('/').adapter.on('join-room', (room, id) => {
  console.log(`-- socket ${id} has joined room ${room}`)
})

io.of('/').adapter.on('leave-room', (room, id) => {
  console.log(`-- socket ${id} has leave room ${room}`)
})

io.of('/').adapter.on('delete-room', room => {
  console.log(`-- room ${room} was deleted`)
})
const getActiveRoomsInternal = setInterval(function () {
  const activeRooms = getActiveRooms()
  console.log('\r\n')
  console.log(activeRooms)
  console.log('\r\n')
  for (let i = 0; i < activeRooms.length; i++) {
    const room = activeRooms[i]
    const usersInRoom = getUsersInRoom(room)
    console.log(`[Total users]: [${room}]: `, usersInRoom)
  }
}, 60000)
async function sendToRoom (rooms, event, message) {
  rooms.forEach(roomID => {
    console.log('emit to room:', roomID, 'event:', event)
    io.sockets.to(roomID).emit(event, { message })
  })
}
const rabbitUrl =
  process.env.RABBIT_URL ||
  'amqp://admin:rDpOP7Wm23fuj5IQT8hiwLZJ@localhost:2093'
console.log('rabbit url:', rabbitUrl)
rabbitConsume(process.env.RABBIT_CONSUME_QUEUE, rabbitUrl, callBackConsume)
async function callBackConsume (message) {
  sendToRoom([process.env.SOCKET_ROOM], process.env.SOCKET_EVENT, message)
}

httpServer.listen(CONST_PORT, err => {
  if (err) {
    clearInterval(getActiveRoomsInternal)
    throw err
  }
  console.log('listening on port ' + CONST_PORT)
})
process.once('SIGINT', async () => {
  console.log('Socket server stopping...')
  process.exit(0)
})
