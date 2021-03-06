﻿using System.Collections;
using System.Collections.Generic;
using Robust.Shared.ViewVariables;

namespace Robust.Client.UserInterface
{
    // ReSharper disable once RequiredBaseTypesIsNotInherited
    public partial class Control
    {
        public const string StylePropertyModulateSelf = "modulate-self";

        private readonly Dictionary<string, object> _styleProperties = new Dictionary<string, object>();
        private readonly HashSet<string> _styleClasses = new HashSet<string>();
        public ICollection<string> StyleClasses { get; }

        private string _styleIdentifier;

        internal int RestyleGeneration;

        [ViewVariables]
        public string StyleIdentifier
        {
            get => _styleIdentifier;
            set
            {
                _styleIdentifier = value;
                Restyle();
            }
        }

        private string _stylePseudoClass;

        [ViewVariables]
        public string StylePseudoClass
        {
            get => _stylePseudoClass;
            protected set
            {
                if (_stylePseudoClass == value)
                {
                    return;
                }

                _stylePseudoClass = value;
                Restyle();
            }
        }

        public bool HasStyleClass(string className)
        {
            return _styleClasses.Contains(className);
        }

        public void AddStyleClass(string className)
        {
            _styleClasses.Add(className);
            Restyle();
        }

        public void RemoveStyleClass(string className)
        {
            _styleClasses.Remove(className);
            Restyle();
        }

        public void SetOnlyStyleClass(string className)
        {
            _styleClasses.Clear();
            _styleClasses.Add(className);
            Restyle();
        }

        private void Restyle()
        {
            _styleProperties.Clear();

            // TODO: probably gonna need support for multiple stylesheets.
            var stylesheet = UserInterfaceManager.Stylesheet;
            if (stylesheet == null)
            {
                return;
            }

            // Get all rules that apply to us, sort them and apply they params again.
            var ruleList = new List<(int index, StyleRule rule)>();

            // Unsorted rules
            foreach (var (index, rule) in stylesheet.UnsortedRules)
            {
                if (rule.Selector.Matches(this))
                {
                    ruleList.Add((index, rule));
                }
            }

            // Rules specific to our type.
            for (var type = GetType(); type != typeof(Control); type = type.BaseType)
            {
                if (!stylesheet.TypeSortedRules.TryGetValue(type, out var typeRuleList))
                {
                    continue;
                }

                foreach (var (index, rule) in typeRuleList)
                {
                    if (rule.Selector.Matches(this))
                    {
                        ruleList.Add((index, rule));
                    }
                }
            }

            // Sort by specificity.
            // The index is there to sort by if specificity is the same, in which case the last takes precedence.
            ruleList.Sort((a, b) =>
            {
                var cmp = a.rule.Specificity.CompareTo(b.rule.Specificity);
                // Reverse this sort so that high specificity is at the TOP.
                return -(cmp != 0 ? cmp : a.index.CompareTo(b.index));
            });

            // Go over each rule.
            foreach (var (_, rule) in ruleList)
            {
                foreach (var property in rule.Properties)
                {
                    if (_styleProperties.ContainsKey(property.Name))
                    {
                        // Since we've sorted by priority in reverse,
                        // the first ones to get applied have highest priority.
                        // So if we have a duplicate it's always lower priority and we can discard it.
                        continue;
                    }

                    _styleProperties[property.Name] = property.Value;
                }
            }

            StylePropertiesChanged();
        }

        protected virtual void StylePropertiesChanged()
        {
            MinimumSizeChanged();
        }

        public bool TryGetStyleProperty<T>(string param, out T value)
        {
            if (_styleProperties.TryGetValue(param, out var val))
            {
                value = (T) val;
                return true;
            }

            value = default;
            return false;
        }

        private sealed class StyleClassCollection : ICollection<string>, IReadOnlyCollection<string>
        {
            private readonly Control _owner;

            public StyleClassCollection(Control owner)
            {
                _owner = owner;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _owner.StyleClasses.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(string item)
            {
                _owner.AddStyleClass(item);
            }

            public void Clear()
            {
                _owner._styleClasses.Clear();
                _owner.Restyle();
            }

            public bool Contains(string item)
            {
                return _owner._styleClasses.Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                _owner._styleClasses.CopyTo(array, arrayIndex);
            }

            public bool Remove(string item)
            {
                var ret = _owner._styleClasses.Remove(item);
                if (ret)
                {
                    _owner.Restyle();
                }

                return ret;
            }

            int ICollection<string>.Count => _owner._styleClasses.Count;
            public bool IsReadOnly => false;
            int IReadOnlyCollection<string>.Count => _owner._styleClasses.Count;
        }
    }
}
