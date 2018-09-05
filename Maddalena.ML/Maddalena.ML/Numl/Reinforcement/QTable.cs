﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maddalena.ML.MachineLearning.Numl.AI;
using Maddalena.ML.MachineLearning.Numl.AI.Collections;
using Maddalena.Numl.Utils;

namespace Maddalena.Numl.Reinforcement
{
    /// <summary>
    /// A Q-Table
    /// </summary>
    public class QTable : SortedTable<IState, IAction, double>
    {
        /// <summary>
        /// Gets or sets the default action to use for unknown states.
        /// </summary>
        public IAction DefaultAction { get; set; }

        /// <summary>
        /// Gets or sets the Q value for the state/action pair identifiers.
        /// </summary>
        /// <param name="state">State identifier.</param>
        /// <param name="action">Action identifier.</param>
        /// <returns>Double.</returns>
        public double this[int state, int action]
        {
            get
            {
                IState s = Keys.FirstOrDefault(f => f.Id == state);
                IAction a = GetKeys(s).FirstOrDefault(f => f.Id == action);

                return base[s, a];
            }
            set
            {
                var s = Keys.FirstOrDefault(f => f.Id == state);
                var a = GetKeys(s).FirstOrDefault(f => f.Id == action);

                AddOrUpdate(s, a, value);
            }
        }

        /// <summary>
        /// Initializes a Q-Table.
        /// </summary>
        public QTable()
        {
            DefaultValue = -0.03;
        }

        /// <summary>
        /// Returns all associated actions for a given state.
        /// </summary>
        /// <param name="state">State.</param>
        /// <returns>IEnumerable&lt;IAction&gt;</returns>
        public IEnumerable<IAction> GetActions(IState state)
        {
            return GetKeys(state);
        }

        /// <summary>
        /// Returns all associated actions for a given state identifier.
        /// <para>Returns <c>null</c> if the state is terminal.</para>
        /// </summary>
        /// <param name="state">State identifier.</param>
        /// <returns>IEnumerable&lt;IAction&gt;</returns>
        public IEnumerable<IAction> GetActions(int state)
        {
            var s = Keys.FirstOrDefault(f => f.Id == state);
            return s != null ? GetActions(s) : null;
        }

        /// <summary>
        /// Returns the action with the heighest Q value for the state.
        /// <para>Returns <c>null</c> if one doesn't exist.</para>
        /// </summary>
        /// <param name="state">State to search.</param>
        /// <returns>IAction.</returns>
        public IAction GetMaxAction(IState state)
        {
            var pairs = GetPairs(state);
            return pairs.Any() ? pairs.OrderByDescending(f => f.Value).Head(s => s.Key) : DefaultAction;
        }

        /// <summary>
        /// Returns the action with the heighest Q value for the state.
        /// <para>Returns <c>-1</c> if the state is terminal.</para>
        /// </summary>
        /// <param name="state">State to search.</param>
        /// <returns>IAction.</returns>
        public int GetMaxAction(int state)
        {
            IState sta = Keys.FirstOrDefault(f => f.Id == state);
            if (sta != null)
            {
                var action = GetMaxAction(sta);
                if (action != null)
                    return action.Id;
            }

            return DefaultAction?.Id ?? -1;
        }
    }
}