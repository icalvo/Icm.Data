Imports Proes.Data

Public Interface IDbRepository(Of TType)
    Inherits IEntityRepository(Of TType)
    Inherits IQueryable(Of TType)

    Function Local() As IEnumerable(Of TType)
    Function Include(ParamArray paths As String()) As IEnumerable(Of TType)
    Function Include(ParamArray includeExpressions As System.Linq.Expressions.Expression(Of Func(Of TType, Object))()) As IQueryable(Of TType)

End Interface

Public Interface IDbRepository(Of TType, TKey)
    Inherits IDbRepository(Of TType)

    Function GetById(ByVal id As Nullable2(Of TKey)) As TType
    Function GetByIdOrCreate(ByVal id As Nullable2(Of TKey), Optional bypassCache As Boolean = False, Optional ByVal includePath As String = Nothing) As TType
    Function DeleteById(ByVal id As TKey) As TType

End Interface

