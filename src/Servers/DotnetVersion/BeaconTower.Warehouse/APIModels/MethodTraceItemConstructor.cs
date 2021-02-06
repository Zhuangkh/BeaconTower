using BeaconTower.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.APIModels
{
    public static class MethodTraceItemConstructor
    {
        public static List<MethodTraceItemResponse> ConstructData(List<MethodTracer> source)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }
            var orderDic = new Dictionary<long, List<MethodTracer>>();
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (!orderDic.TryGetValue(item.PreMethodEventID, out var targetList))
                {
                    targetList = new List<MethodTracer>();
                    orderDic.Add(item.PreMethodEventID, targetList);
                }
                targetList.Add(item);
            }
            var resDic = new Dictionary<long, List<MethodTraceItemResponse>>();

            foreach (var item in orderDic)
            {
                var groupData = item.Value.GroupBy(item => item.MethodEventID).ToList();
                if (!resDic.ContainsKey(item.Key))
                {
                    resDic.Add(item.Key, new List<MethodTraceItemResponse>());
                }
                for (int i = 0; i < groupData.Count; i++)
                {
                    var thisLoop = groupData[i].OrderBy(item => item.TimeStamp).ToList();
                    var model = new MethodTraceItemResponse();
                    if (thisLoop.Count > 0)
                    {
                        var begin = thisLoop[0];
                        model.TraceID = begin.TraceID;
                        model.NodeID = begin.NodeID;
                        model.EventID = begin.EventID;
                        model.MethodEventID = begin.MethodEventID;
                        model.PreMethodEventID = begin.PreMethodEventID;
                        model.MethodID = begin.MethodID;
                        model.MethodName = begin.MethodName;
                        model.FileName = begin.FileName;
                        model.LineNumber = begin.LineNumber;
                        model.BeginCustomData = begin.CustomData;
                        model.BeginTimeStamp = begin.TimeStamp;
                    }
                    if (thisLoop.Count > 1)
                    {
                        var end = thisLoop[1];
                        model.EndCustomData = end.CustomData;
                        model.EndTimeStamp = end.TimeStamp;
                    }
                    resDic[item.Key].Add(model);
                }
            }


            var root = ConstructByData(resDic, null);

            return root.Children;
        }

        private static MethodTraceItemResponse ConstructByData(
            Dictionary<long, List<MethodTraceItemResponse>> data,
            MethodTraceItemResponse parent
            )
        {
            if (parent == null)
            {
                parent = new MethodTraceItemResponse()
                {                    
                    MethodEventID = 0
                };
            }
            if (data.TryGetValue(parent.MethodEventID, out var nextNodes))
            {
                parent.Children.AddRange(nextNodes);
                for (int i = 0; i < nextNodes.Count; i++)
                {
                    ConstructByData(data, nextNodes[i]);
                }
            }
            return parent;
        }
    }
}
