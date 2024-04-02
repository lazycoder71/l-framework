using System.Collections.Generic;

namespace LFramework
{
    public static class LCollectDestinationHelper
    {
        static Dictionary<LCollectConfig, Stack<LCollectDestination>> destinationDict;

        public static void Push(LCollectDestination destination)
        {
            if (destinationDict == null)
                destinationDict = new Dictionary<LCollectConfig, Stack<LCollectDestination>>();

            if (!destinationDict.ContainsKey(destination.config))
                destinationDict.Add(destination.config, new Stack<LCollectDestination>());

            destinationDict[destination.config].Push(destination);
        }

        public static void Pop(LCollectDestination destination)
        {
            if (destinationDict == null)
                return;

            Stack<LCollectDestination> stack;

            destinationDict.TryGetValue(destination.config, out stack);

            if (stack == null || stack.Count == 0)
                return;

            stack.Pop();
        }

        public static LCollectDestination Get(LCollectConfig config)
        {
            if (destinationDict == null)
                return null;

            if (!destinationDict.ContainsKey(config))
                return null;

            Stack<LCollectDestination> stack = destinationDict[config];

            if(stack == null || stack.Count == 0) 
                return null;

            return stack.Peek();
        }
    }
}
