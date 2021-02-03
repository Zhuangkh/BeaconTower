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
            var orderDic = new Dictionary<string, List<NodeTracer>>();
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (!orderDic.TryGetValue(item.PreviousNodeID, out var targetList))
                {
                    targetList = new List<NodeTracer>();
                    orderDic.Add(item.PreviousNodeID ?? "", targetList);
                }
                targetList.Add(item);
            }


            return ConstructByData("", orderDic);


            //source = source.OrderBy(item => item.TimeStamp).ToList();
            //var root = ConstructRoot(source);
            //ConstructChildData(source, root);
            //return root;
        }

        private static NodeTraceItemResponse ConstructByData(
            string previousNodeID,
            Dictionary<string, List<NodeTracer>> orderDic)
        {
            //var rootInfo = orderDic[previousNodeID].OrderBy(item => item.TimeStamp).ToList();
            var groupData = orderDic[previousNodeID].GroupBy(item => item.EventID).ToList();

            var i = 0;
            return i == 0 ? null : null;
            //for (int i = 0; i < groupData.Count; i++)
            //{
            //    var thisLoop=groupData[i];

            //}


            //var model = new NodeTraceItemResponse();
            //if (data.Count > 0)
            //{
            //    var begin = data[0];
            //    model.TraceID = begin.TraceID;
            //    model.NodeID = begin.NodeID;
            //    model.Path = begin.Path;
            //    model.BeginCustomData = begin.CustomData;
            //    model.BeginTimeStamp = begin.TimeStamp;
            //    model.QueryString = begin.QueryString;
            //    model.PreviousNodeID = string.Empty;
            //    model.Type = begin.Type;
            //}
            //if (data.Count > 1)
            //{
            //    var end = data[1];
            //    model.EndCustomData = end.CustomData;
            //    model.EndTimeStamp = end.TimeStamp;
            //}
            //return model;
        }

        //Todo: you can improve performance here...
        private static NodeTraceItemResponse ConstructRoot(List<NodeTracer> source)
        {
            if (source == null)
            {
                return null;
            }
            var targetIndex = new List<int>();
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (string.IsNullOrWhiteSpace(item.PreviousNodeID))
                {
                    targetIndex.Add(i);
                }
            }
            NodeTracer begin = null;
            NodeTracer end = null;
            if (targetIndex.Count > 0)
            {
                begin = source[targetIndex[0]];
            }
            if (targetIndex.Count == 2)
            {
                end = source[targetIndex[1]];
            }
            var root = new NodeTraceItemResponse()
            {
                TraceID = begin.TraceID,
                NodeID = begin.NodeID,
                Path = begin.Path,
                BeginCustomData = begin.CustomData,
                BeginTimeStamp = begin.TimeStamp,
                QueryString = begin.QueryString,
                PreviousNodeID = string.Empty,
                Type = begin.Type
            };
            if (end != null)
            {
                root.EndCustomData = end.CustomData;
                root.EndTimeStamp = end.TimeStamp;
            }
            source.Remove(begin);
            if (end != null)
            {
                source.Remove(end);
            }
            return root;
        }

        //Todo: you can improve performance here...
        private static void ConstructChildData(List<NodeTracer> source, NodeTraceItemResponse parentNode)
        {
            if (source == null || source.Count == 0)
            {
                return;
            }
            if (parentNode == null || parentNode.NextNode == null)
            {
                return;
            }
            List<NodeTracer> allChild = new List<NodeTracer>();
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].PreviousNodeID.Equals(parentNode.NodeID))
                {
                    allChild.Add(source[i]);
                }
            }
            for (int i = 0; i < allChild.Count; i++)
            {
                var item = allChild[i];
                source.Remove(item);
                var target = parentNode.NextNode.FirstOrDefault(nItem => nItem.NodeID == item.NodeID);
                if (target == null)
                {

                    target = new NodeTraceItemResponse()
                    {
                        BeginCustomData = item.CustomData,
                        BeginTimeStamp = item.TimeStamp,
                        NodeID = item.NodeID,
                        Path = item.Path,
                        PreviousNodeID = parentNode.NodeID,
                        QueryString = item.QueryString,
                        TraceID = item.TraceID,
                        Type = item.Type,
                    };

                    parentNode.NextNode.Add(target);
                }
                else
                {
                    target.EndCustomData = item.CustomData;
                    target.EndTimeStamp = item.TimeStamp;
                }
            }
            for (int i = 0; i < parentNode.NextNode.Count; i++)
            {
                ConstructChildData(source, parentNode.NextNode[i]);
            }

        }
    }
}
