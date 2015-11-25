Imports System.Runtime.CompilerServices

Public Module IDbRepositoryExtensions

    <Extension>
    Public Function GetById(Of TType, TKey As Structure)(repo As IDbRepository(Of TType, TKey), ByVal id As TKey?) As TType
        Return repo.GetById(id.ToNullable2)
    End Function


    <Extension>
    Public Function GetLocalFirst(Of TType)(repo As IDbRepository(Of TType), fn As Func(Of IQueryable(Of TType), TType)) As TType
        Dim localObj = fn(repo.Local.AsQueryable)
        If localObj Is Nothing Then
            Return fn(repo)
        End If
    End Function

End Module
