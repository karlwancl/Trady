using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class TickProviderBase : ITickProvider
    {
        public abstract bool IsReady { get; }
        protected object[] _parameters;
        protected IAnalyzable _analyzable;

        public TickProviderBase(params object[] parameters)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Shallow clone the tick provider
        /// </summary>
        /// <returns>Tick provider</returns>
        public ITickProvider Clone()
        {
            var ctor = GetType().GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            return (ITickProvider)ctor.Invoke(_parameters);
        }

        /// <summary>
        /// Get the dataset of ticks from external data source from start time
        /// </summary>
        /// <typeparam name="TTick">Type of tick</typeparam>
        /// <param name="startTime">Start time</param>
        /// <returns>List of ticks task</returns>
        public async Task<IEnumerable<TTick>> GetAsync<TTick>(DateTime startTime) where TTick : ITick
        {
            if (IsReady)
            {
                var ticks = new List<TTick>();
                var valueTicks = await GetValueTicks(startTime).ConfigureAwait(false);
                if (!valueTicks.Any())
                    return ticks;

                var valueTicksGroups = valueTicks.GroupBy(t => t.DateTime);

                var ctor = typeof(TTick).GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).First();

                foreach (var valueTicksGroup in valueTicksGroups)   // Loop by datetime
                {
                    var args = new List<object> { valueTicksGroup.Key };    // The first one must be datetime
                    foreach (var @param in ctor.GetParameters())    // Loop by TTick ctor parameters
                    {
                        // Compare parameter name with IndicatorValue name field, add to arguments if matched
                        if (@param.Name.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                            continue;
                        var propTick = valueTicksGroup.FirstOrDefault(t => @param.Name.Replace("@", "").Equals(t.Name, StringComparison.OrdinalIgnoreCase));
                        if (propTick != null)
                        {
                            object obj = null;
                            if (propTick.Value != null)
                            {
                                var paramType = Nullable.GetUnderlyingType(@param.ParameterType) ?? @param.ParameterType;
                                obj = paramType.GetTypeInfo().IsEnum ?
                                    Enum.ToObject(paramType, propTick.Value) :
                                    Convert.ChangeType(propTick.Value, paramType);
                            }
                            args.Add(obj);
                        }
                    }

                    // Construct and add TTick
                    var tick = (TTick)ctor.Invoke(args.ToArray());
                    ticks.Add(tick);
                }

                return ticks;
            }
            return new List<TTick>();
        }

        /// <summary>
        /// Get each tick from external data source with the format: (name, value)
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <returns>List of (name, value)</returns>
        protected abstract Task<IEnumerable<IValueTick>> GetValueTicks(DateTime startTime);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Initialize with analyzable, you should call base method when override this method
        /// </summary>
        /// <param name="analyzable">Analyzable</param>
        /// <returns>Task object</returns>
        public async virtual Task InitWithAnalyzableAsync(IAnalyzable analyzable)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _analyzable = analyzable;
            // Implement your own logic here
        }
    }
}