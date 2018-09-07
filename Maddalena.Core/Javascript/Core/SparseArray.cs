using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Maddalena.Core.Javascript.Core
{
    public enum ArrayMode
    {
        Flat,
        Sparse
    }

    public sealed class SparseArray<TValue> : IList<TValue>
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct _NavyItem
        {
            public uint index;
            public uint zeroContinue;
            public uint oneContinue;

            public override string ToString()
            {
                return index + "[" + zeroContinue + ";" + oneContinue + "]";
            }
        }

        private static readonly _NavyItem[] emptyNavyData = new _NavyItem[0]; // data dummy. In cases where instance of current class was created, but not used
        private static readonly TValue[] emptyData = new TValue[0];

        private uint _allocatedCount;
        private _NavyItem[] _navyData;
        private TValue[] _values;
        private bool _zeroExists;

        public ArrayMode Mode { get; private set; }


        public uint Length { get; private set; }

        public SparseArray(ArrayMode arrayMode = ArrayMode.Flat)
        {
            Mode = arrayMode;
            _values = emptyData;
            _navyData = emptyNavyData;
        }

        public SparseArray(int capacity)
        {
            Mode = ArrayMode.Flat;
            _values = emptyData;
            _navyData = emptyNavyData;
            if (capacity > 0)
                ensureCapacity(capacity);
        }

        public SparseArray(TValue[] values)
        {
            Mode = ArrayMode.Flat;
            _values = values;
            _navyData = emptyNavyData;
            _allocatedCount = (Length = (uint)values.Length);
        }

        #region Члены IList<TValue>

        public int IndexOf(TValue item)
        {
            for (var i = 0; i < _allocatedCount; i++)
            {
                if (Equals(_values[i], item))
                {
                    if (Mode == ArrayMode.Flat)
                        return i;
                    return (int)_navyData[i].index;
                }
            }

            return -1;
        }

        void IList<TValue>.Insert(int index, TValue item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If "index" not equal "Length - 1", method will fail
        /// </summary>
        /// <param name="index">Index of item for removing</param>
        public void RemoveAt(int index)
        {
            if (Length == 0 || index != (int)(Length - 1))
                throw new InvalidOperationException();
            this[(int)(Length - 1)] = default(TValue);
            Length--;
        }

        public TValue this[int index]
        {
            get
            {
                if (Mode == ArrayMode.Flat)
                {
                    if (index < 0 || Length <= index || _values.Length <= index)
                        return default(TValue);

                    return _values[index];
                }

                if (_navyData.Length == 0)
                    return default(TValue);

                uint unsignedIndex = (uint)index;
                int bi = 31;
                uint i = 0;

                if (unsignedIndex < _allocatedCount)
                {
                    if (_navyData[index].index == unsignedIndex)
                        return _values[index];
                }

                for (; ; bi--)
                {
                    i = (unsignedIndex & (1 << bi)) == 0 ? _navyData[i].zeroContinue : _navyData[i].oneContinue;
                    if (i == 0)
                    {
                        return default(TValue);
                    }

                    if (_navyData[i].index == unsignedIndex)
                    {
                        return _values[i];
                    }
                }
            }
            set
            {
                bool @default = value == null; // структуры мы будем записывать, иначе пришлось бы вызывать тяжелые операции сравнения.
                //if (navyData.Length <= allocatedCount)
                //    ensureCapacity(navyData.Length * 2);
                uint unsignedIndex = (uint)index;
                if (Mode == ArrayMode.Flat)
                {
                    if (index < 0 || index > Length)
                    {
                        if (@default)
                        {
                            if (unsignedIndex >= Length)
                                Length = unsignedIndex + 1;
                            return;
                        }

                        if (unsignedIndex < 32)
                        {
                            // Покрывает много тех случаев, когда относительно маленький массив заполняют с конца.
                            // Кто-то верит, что это должно работать быстрее.
                            // Вот именно из-за таких кусков кода так и может показаться.
                            // Не время для попыток исправить мир
                            Length = unsignedIndex + 1;
                            ensureCapacity((int)Length);
                            this[index] = value;
                            return;
                        }

                        RebuildToSparse();
                    }
                    else
                    {
                        if (_values.Length <= index)
                            ensureCapacity(Math.Max(index + 1, _values.Length * 2));
                        if (Length == index)
                            Length = unsignedIndex + 1;
                        _values[index] = value;
                        return;
                    }
                }

                if (_allocatedCount == 0)
                {
                    ensureCapacity(1);
                    _allocatedCount = 1;
                }

                if (unsignedIndex < _allocatedCount)
                {
                    if (_navyData[index].index == unsignedIndex)
                    {
                        if (index == 0)
                            _zeroExists = true;

                        if (Length <= index)
                            Length = unsignedIndex + 1;

                        _values[index] = value;

                        return;
                    }
                }

                int bi = 31;
                for (uint i = 0, ni = 0; ; bi--)
                {
                    if (_navyData[i].index > unsignedIndex)
                    {
                        if (@default)
                        {
                            if (Length <= unsignedIndex)
                                Length = unsignedIndex + 1; // длина может быть меньше
                            // уже записанных элементов если эти элементы имеют значение
                            // по-умолчанию и был вызван Trim
                            return;
                        }

                        var oi = _navyData[i].index;
                        var ov = _values[i];
                        _navyData[i].index = unsignedIndex;
                        _values[i] = value;
                        if (oi < Length)
                            this[(int)oi] = ov;
                        return;
                    }

                    if (_navyData[i].index < unsignedIndex)
                    {
                        var b = (unsignedIndex & (1 << bi)) == 0;
                        ni = b ? _navyData[i].zeroContinue : _navyData[i].oneContinue;
                        if (ni == 0)
                        {
                            if (Length <= unsignedIndex)
                                Length = unsignedIndex + 1;

                            if (@default)
                                return;

                            if (b)
                                _navyData[i].zeroContinue = ni = _allocatedCount++;
                            else
                                _navyData[i].oneContinue = ni = _allocatedCount++;

                            if (_navyData.Length <= _allocatedCount)
                                ensureCapacity(_navyData.Length * 2);

                            _navyData[ni].index = unsignedIndex;
                            _values[ni] = value;
                            return;
                        }

                        i = ni;
                    }
                    else
                    {
                        _values[i] = value;
                        if (Length <= index)
                            Length = unsignedIndex + 1;
                        if (_allocatedCount <= i)
                            _allocatedCount = i + 1;
                        return;
                    }
                }
            }
        }

        #endregion

        #region Члены ICollection<TValue>

        public void Add(TValue item)
        {
            if (Length == uint.MaxValue)
                throw new InvalidOperationException();

            this[(int)(Length)] = item;
        }

        public void Clear()
        {
            while (_allocatedCount > 0)
            {
                _navyData[(int)(--_allocatedCount)] = default(_NavyItem);
                _values[(int)_allocatedCount] = default(TValue);
            }
            Length = 0;
        }

        public bool Contains(TValue item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            if (array == null)
                throw new NullReferenceException();
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (Math.Min(Length, int.MaxValue) - arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException();

            for (var i = Math.Min(Length, int.MaxValue) + arrayIndex; i-- > arrayIndex;)
            {
                array[i] = default(TValue);
            }

            foreach (var v in DirectOrder)
            {
                if (v.Key >= 0)
                    array[v.Key + arrayIndex] = v.Value;
            }
        }

        int ICollection<TValue>.Count => (int)Length;

        public bool IsReadOnly => false;

        public bool Remove(TValue item)
        {
            throw new NotImplementedException();
        }

        #endregion

        public IEnumerator<TValue> GetEnumerator()
        {
            for (var i = 0u; i < Length; i++)
                yield return this[(int)i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Zero if the requested index does not Exists</returns>
        public long NearestIndexNotLess(long index)
        {
            if (Mode == ArrayMode.Sparse)
            {
                if (_navyData.Length == 0)
                    return index;
            }
            else
            {
                if (_values.Length < index)
                    return 0;
                return index;
            }
            int bi = 31;
            long i = 0;
            long pm = -1;
            for (; ; bi--)
            {
                if (_navyData[i].oneContinue != 0)
                    pm = i;
                i = (index & (1 << bi)) == 0 ? _navyData[i].zeroContinue : _navyData[i].oneContinue;
                if (i == 0)
                {
                    if (pm == -1)
                        return 0;
                    i = _navyData[pm].oneContinue;
                    for (; ; )
                    {
                        if (_navyData[i].zeroContinue != 0)
                        {
                            i = _navyData[i].zeroContinue;
                            continue;
                        }
                        if (_navyData[i].oneContinue != 0)
                        {
                            i = _navyData[i].oneContinue;
                            continue;
                        }
                        break;
                    }
                }
                if (_navyData[i].index >= (uint)index)
                {
                    return _navyData[i].index;
                }
            }
        }

        public long NearestIndexNotMore(long index)
        {
            if (Mode == ArrayMode.Sparse)
            {
                if (_navyData.Length == 0)
                    return 0;
            }
            else
            {
                return Math.Min(_values.Length, index);
            }
            int bi = 31;
            long i = 0;
            for (; ; bi--)
            {
                var ni = (index & (1 << bi)) == 0 ? _navyData[i].zeroContinue : _navyData[i].oneContinue;
                if (ni == 0 || _navyData[ni].index > index)
                    index = _navyData[i].index;
                else
                    i = ni;
                if (_navyData[i].index == index)
                {
                    return _navyData[i].index;
                }
            }
        }

        public IEnumerable<KeyValuePair<int, TValue>> DirectOrder
{
            get
            {
                var index = 1U;
                var skipFirst = !_zeroExists;
                if (Mode == ArrayMode.Flat)
                {
                    for (var i = 0; i < Length; i++)
                    {
                        skipFirst = true;
                        if (i >= _values.Length) // была насильно установлена длина фиктивным элементом.
                        {
                            yield return new KeyValuePair<int, TValue>((int)(Length - 1), default(TValue));
                            yield break;
                        }

                        yield return new KeyValuePair<int, TValue>(i, _values[i]);

                        if (Mode != ArrayMode.Flat)
                        {
                            index = (uint)(i + 1);
                            break;
                        }
                    }
                }

                if (Mode == ArrayMode.Sparse) // Режим может поменяться во время итерации в режиме Flat
                {
                    if (_allocatedCount > 0)
                    {
                        if (!skipFirst)
                        {
                            yield return new KeyValuePair<int, TValue>(0, _values[0]);
                        }
                    }
                    else
                    {
                        yield return new KeyValuePair<int, TValue>((int)(Length - 1), default(TValue));
                        yield break;
                    }

                    while (index < Length)
                    {
                        int bi = 31;
                        long i = 0;
                        long pm = -1;
                        for (; bi >= 0; bi--)
                        {
                            var goToZero = (index & (1U << bi)) == 0;
                            if (goToZero && _navyData[i].oneContinue != 0)
                                pm = _navyData[i].oneContinue;

                            i = goToZero ? _navyData[i].zeroContinue : _navyData[i].oneContinue;

                            if (i == 0)
                            {
                                if (pm == -1)
                                {
                                    yield return new KeyValuePair<int, TValue>((int)(Length - 1), default(TValue));
                                    yield break;
                                }

                                i = pm;
                                for (; ; )
                                {
                                    if (_navyData[i].zeroContinue != 0)
                                    {
                                        i = _navyData[i].zeroContinue;
                                        continue;
                                    }
                                    if (_navyData[i].oneContinue != 0)
                                    {
                                        i = _navyData[i].oneContinue;
                                        continue;
                                    }
                                    break;
                                }
                            }

                            if (_navyData[i].index >= index)
                            {
                                index = _navyData[i].index;
                                yield return new KeyValuePair<int, TValue>((int)index, _values[i]);
                                index++;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<KeyValuePair<int, TValue>> ReversOrder
        {
            get
            {
                var index = Length - 1;
                if (Mode == ArrayMode.Flat)
                {
                    if (Length > _values.Length)
                        yield return new KeyValuePair<int, TValue>((int)(Length - 1), default(TValue));
                    for (var i = Math.Min(_values.Length, Length); i-- > 0;)
                    {
                        if (Mode != ArrayMode.Flat)
                        {
                            index = (uint)i;
                            break;
                        }
                        yield return new KeyValuePair<int, TValue>((int)i, _values[i]);
                    }
                }
                if (Mode == ArrayMode.Sparse)
                {
                    if (_allocatedCount == 0)
                        yield break;
                    while (index > 0)
                    {
                        int bi = 31;
                        long i = 0;
                        for (; ; bi--)
                        {
                            var ni = (index & (1 << bi)) == 0 ? _navyData[i].zeroContinue : _navyData[i].oneContinue;
                            if (ni == 0 || _navyData[ni].index > index)
                                index = _navyData[i].index;
                            else
                                i = ni;
                            if (_navyData[i].index == index)
                            {
                                yield return new KeyValuePair<int, TValue>((int)index, _values[i]);
                                if (index == 0)
                                    yield break;
                                index--;
                                break;
                            }
                        }
                    }
                    yield return new KeyValuePair<int, TValue>(0, _values[0]);
                }
            }
        }

        /// <summary>
        /// Reduce length to "index of last item with non-default value" + 1
        /// </summary>
        public void Trim()
        {
            long len = -1;
            if (Mode == ArrayMode.Flat)
            {
                for (var i = _values.Length; i-- > 0;)
                {
                    if (!Equals(_values[i], default(TValue)))
                    {
                        len = i;
                        break;
                    }
                }
            }
            else
            {
                for (var i = _allocatedCount; i-- > 0;)
                {
                    if (_navyData[i].index > len && !Equals(_values[i], default(TValue)))
                        len = _navyData[i].index;
                }
            }
            Length = (uint)(len + 1);
        }

        private void ensureCapacity(int p)
        {
            p = Math.Max(4, p);
            if (_values.Length >= p)
                return;

            var newValues = new TValue[p];
            if (_values != null)
                for (var i = 0; i < _values.Length; i++)
                    newValues[i] = _values[i];
            _values = newValues;
            if (Mode == ArrayMode.Sparse)
            {
                var newData = new _NavyItem[p];
                for (var i = 0; i < _navyData.Length; i++)
                    newData[i] = _navyData[i];
                _navyData = newData;
            }
        }

        public void RebuildToSparse()
        {
            _allocatedCount = 0;
            Mode = ArrayMode.Sparse;
            var len = Length;
            if (len == 0)
            {
                ensureCapacity(0);
                return;
            }

            _navyData = new _NavyItem[_values.Length];
            var data = _values;
            _values = new TValue[_values.Length];
            len = (uint)Math.Min(data.Length, len);
            for (var i = 0; i < len; i++)
                this[i] = data[i];

            if (_values.Length < len)
                this[(int)len - 1] = default(TValue);
        }
    }
}
