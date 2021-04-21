using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data.SqlHelper
{
    /// <summary>
    /// Represents the extensions for MultipleResultSets
    /// </summary>
    public static class StoredProcBuilderMultipleResultSetsExtention
    {
        public static (IEnumerable<T1> FirstResultSet, IEnumerable<T2> SecondResultSet) 
            ExecMulti<T1, T2>(this IStoredProcBuilder storedProcBuilder)
        {
            storedProcBuilder.ClearResult();
            storedProcBuilder.ResultSets = new List<Type> { typeof(T1), typeof(T2) };

            var resultSets = storedProcBuilder.ExecMulti();

            return ((IEnumerable<T1>)resultSets[0], (IEnumerable<T2>)resultSets[1]);
        }
        public static (IEnumerable<T1> FirstResultSet, IEnumerable<T2> SecondResultSet, 
            IEnumerable<T3> ThirdResultSet) 
            ExecMulti<T1, T2, T3>
            (this IStoredProcBuilder storedProcBuilder)
        {
            storedProcBuilder.ClearResult();
            storedProcBuilder.ResultSets = new List<Type> { typeof(T1), typeof(T2), typeof(T3) };

            var resultSets = storedProcBuilder.ExecMulti();

            return ((IEnumerable<T1>)resultSets[0], (IEnumerable<T2>)resultSets[1], (IEnumerable<T3>)resultSets[2]);
        }
        public static (IEnumerable<T1> FirstResultSet, IEnumerable<T2> SecondResultSet, 
            IEnumerable<T3> ThirdResultSet, IEnumerable<T4> FourthResultSet) 
            ExecMulti<T1, T2, T3, T4>(this IStoredProcBuilder storedProcBuilder)
        {
            storedProcBuilder.ClearResult();
            storedProcBuilder.ResultSets = new List<Type> { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };

            var resultSets = storedProcBuilder.ExecMulti();

            return ((IEnumerable<T1>)resultSets[0], (IEnumerable<T2>)resultSets[1], (IEnumerable<T3>)resultSets[2], (IEnumerable<T4>)resultSets[3]);
        }
        public static (IEnumerable<T1> FirstResultSet, IEnumerable<T2> SecondResultSet, 
            IEnumerable<T3> ThirdResultSet, IEnumerable<T4> FourthResultSet, IEnumerable<T5> FifthResultSet) 
            ExecMulti<T1, T2, T3, T4, T5>(this IStoredProcBuilder storedProcBuilder)
        {
            storedProcBuilder.ClearResult();
            storedProcBuilder.ResultSets = new List<Type> { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };

            var resultSets = storedProcBuilder.ExecMulti();

            return ((IEnumerable<T1>)resultSets[0], (IEnumerable<T2>)resultSets[1], (IEnumerable<T3>)resultSets[2], (IEnumerable<T4>)resultSets[3], (IEnumerable<T5>)resultSets[4]);
        }
    }
}
