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
            source = source.OrderBy(item => item.TimeStamp).ToList();
            var root = ConstructRoot(source);
            ConstructChildData(source, root);
            return root;
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
            if (targetIndex.Count > 1)
            {
                begin = source[targetIndex[0]];
            }
            if (targetIndex.Count == 2)
            {
                end = source[targetIndex[1]];
            }
            source.Remove(begin);
            if (end != null)
            {
                source.Remove(end);
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
