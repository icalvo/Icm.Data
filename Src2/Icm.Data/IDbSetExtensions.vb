Imports System.Runtime.CompilerServices
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports Icm.Reflection


Public Module IDbSetExtensions

    <Extension>
    Public Function Copy(Of TType As Class)(dbset As IDbSet(Of TType), other As TType) As TType
        Dim newObject = dbset.Create()
        other.CopyEntityTo(newObject, "id")
        dbset.Add(newObject)

        Return newObject
    End Function

    <Extension>
    Public Function GetById(Of TKey, TType As Class)(dbset As IDbSet(Of TType), ByVal id As Nullable2(Of TKey)) As TType
        If id.HasValue Then
            Return TryCast(dbset.Find(id.Value), TType)
        Else
            Return Nothing
        End If
    End Function

    <Extension>
    Public Function GetByIdOrCreate(Of TKey, TType As Class)(dbset As IDbSet(Of TType), ByVal id As Nullable2(Of TKey), Optional bypassCache As Boolean = False, Optional ByVal includePath As String = Nothing) As TType
        Dim obj = dbset.GetById(id)
        If obj Is Nothing Then
            obj = dbset.Create()
        End If
        Return obj
    End Function

    <Extension>
    Public Function RemoveById(Of TKey, TType As Class)(dbset As IDbSet(Of TType), ByVal id As TKey) As TType
        Dim result = dbset.GetById(Of TKey)(id)
        dbset.Remove(result)

        Return result
    End Function

    <Extension>
    Public Function Include(Of TType As Class)(dbset As IDbSet(Of TType), ParamArray includeExpressions As Expression(Of Func(Of TType, Object))()) As IQueryable(Of TType)
        Dim query = dbset.AsQueryable.OfType(Of TType)()
        For Each includeExpression In includeExpressions
            query = query.Include(includeExpression)
        Next
        Return query
    End Function

    <Extension>
    Public Function Include(Of TType As Class)(dbset As IDbSet(Of TType), ParamArray paths As String()) As IEnumerable(Of TType)
        Dim query = dbset.AsQueryable
        For Each path In paths
            query = query.Include(path)
        Next
        Return query.OfType(Of TType)()
    End Function

    <Extension>
    Public Function Local(Of TType As Class, TDerivedType As TType)(dbset As IDbSet(Of TType)) As IEnumerable(Of TDerivedType)
        Return dbset.Local.OfType(Of TDerivedType)()
    End Function

    <Extension>
    Public Function GetById(Of TKey As Structure, TType As Class)(repo As IDbSet(Of TType), ByVal id As TKey?) As TType
        Return repo.GetById(Of TKey)(id.ToNullable2)
    End Function

    <Extension>
    Public Function GetLocalFirst(Of TType As Class)(repo As IDbSet(Of TType), fn As Func(Of IQueryable(Of TType), TType)) As TType
        Dim localObj = fn(repo.Local.AsQueryable)
        If localObj Is Nothing Then
            Return fn(repo)
        End If
        Return localObj
    End Function


End Module
