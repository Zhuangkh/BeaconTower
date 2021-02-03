using BeaconTower.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.APIModels
{
    /// <summary>
    /// Must use ordered list.....you can improve performance here
    /// you can improve performance here
    /// you can improve performance here
    /// you can improve performance here
    /// you can improve performance here
    /// you can improve performance here
    /// </summary>
    public static class NodeTraceItemConstructor
    {
        public static NodeTraceItemResponse ConstructData(List<NodeTracer> source)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }
            var orderDic = new Dictionary<long, List<NodeTracer>>();
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (!orderDic.TryGetValue(item.PreviousEventID, out var targetList))
                {
                    targetList = new List<NodeTracer>();
                    orderDic.Add(item.PreviousEventID, targetList);
                }
                targetList.Add(item);
            }
            var resDic = new Dictionary<long, List<NodeTraceItemResponse>>();

            foreach (var item in orderDic)
            {
                var groupData = item.Value.GroupBy(item => item.EventID).ToList();
                if (!resDic.ContainsKey(item.Key))
                {
                    resDic.Add(item.Key, new List<NodeTraceItemResponse>());
                }
                for (int i = 0; i < groupData.Count; i++)
                {
                    var thisLoop = groupData[i].OrderBy(item => item.TimeStamp).ToList();
                    var model = new NodeTraceItemResponse();
                    if (thisLoop.Count > 0)
                    {
                        var begin = thisLoop[0];
                        model.TraceID = begin.TraceID;
                        model.NodeID = begin.NodeID;
                        model.Path = begin.Path;
                        model.EventID = begin.EventID;
                        model.BeginCustomData = begin.CustomData;
                        model.BeginTimeStamp = begin.TimeStamp;
                        model.QueryString = begin.QueryString;
                        model.PreviousNodeID = string.Empty;
                        model.Type = begin.Type;
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

            return root; 
        }

        private static NodeTraceItemResponse ConstructByData(
            Dictionary<long, List<NodeTraceItemResponse>> data,
            NodeTraceItemResponse parent
            )
        {
            if (parent == null)
            {
                parent = data[0][0];
                ConstructByData(data, parent);
                return parent;
            }
            else if (data.TryGetValue(parent.EventID,out var nextNodes))
            { 
                parent.NextNode.AddRange(nextNodes);
                for (int i = 0; i < nextNodes.Count; i++)
                {
                    ConstructByData(data, nextNodes[i]);
                }
            }
            return null;
        }

    }
}
