using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.MetricsNS
{
  public class GaugeMetrics
  {
    private static readonly Dictionary<string, Gauge> dicGaugeCounter = new Dictionary<string, Gauge>();
    private static readonly Dictionary<string, Gauge> dicGaugeRate = new Dictionary<string, Gauge>();
    public static void TrackingCounterValue(string metricName, string[] tagsTitle, string[] tagsValue, double value)
    {
      lock (dicGaugeCounter)
      {
        if (!dicGaugeCounter.ContainsKey(metricName))
        {
          Gauge gauge = Metrics.CreateGauge(metricName, metricName, new GaugeConfiguration
          {
            LabelNames = tagsTitle
          });

          dicGaugeCounter.Add(metricName, gauge);
        }
      }

      dicGaugeCounter[metricName].WithLabels(tagsValue).Inc(value);
    }
    public static void TrackingRateValue(string metricName, string[] tagsTitle, string[] tagsValue, double value)
    {
      lock (dicGaugeRate)
      {
        if (!dicGaugeRate.ContainsKey(metricName))
        {
          Gauge gauge = Metrics.CreateGauge(metricName, metricName, new GaugeConfiguration
          {
            LabelNames = tagsTitle
          });

          dicGaugeRate.Add(metricName, gauge);
        }
      }

      dicGaugeRate[metricName].WithLabels(tagsValue).Inc(value);
    }
    public static void TrackMetricsPushMessage(string exchangeNameOrQueueName, bool isOk = true)
    {
      TrackingCounterValue("message_queue_total_" + exchangeNameOrQueueName.RemoveSpecialChar().ToLower(),
        new string[] { "action", "type" },
        new string[] { "push", isOk ? "push" : "error" },
        1);
      TrackingRateValue("message_queue_rate_1m_" + exchangeNameOrQueueName.RemoveSpecialChar().ToLower(),
        new string[] { "action", "type" },
        new string[] { "push", isOk ? "push" : "error" },
        1);
    }
    public static void TrackMetricsReceiveMessage(string queueName, bool isAck, bool isRetry = false, bool isStopRetry = false)
    {
      TrackingCounterValue("message_queue_total_" + queueName.RemoveSpecialChar().ToLower(),
        new string[] { "action", "type" },
        new string[] { "receive", isAck ? "ack" : (isRetry ? (isStopRetry ? "stop" : "retry") : "error") },
        1);
      TrackingRateValue("message_queue_rate_1m_" + queueName.RemoveSpecialChar().ToLower(),
        new string[] { "action", "type" },
        new string[] { "receive", isAck ? "ack" : (isRetry ? (isStopRetry ? "stop" : "retry") : "error") },
        1);
    }
    public static void ResetRateMetrics()
    {
      lock (dicGaugeRate)
      {
        foreach (var keyVal in dicGaugeRate)
        {
          foreach (var labelValues in keyVal.Value.GetAllLabelValues())
          {
            keyVal.Value.WithLabels(labelValues).Set(0);
          }
        }
      }
    }
  }
}
