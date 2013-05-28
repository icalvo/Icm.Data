Imports System.Data.Objects
Imports System.Runtime.CompilerServices
Imports System.Data.Entity.Infrastructure

Public Module IEnumerableExtensions

    <Extension()>
    Public Function Include(Of TSource)(ByVal source As IEnumerable(Of TSource), ParamArray path() As String) As IEnumerable(Of TSource)
        Dim objectQuery = TryCast(source, ObjectQuery(Of TSource))
        If objectQuery IsNot Nothing Then
            Return objectQuery.Include(path)
        End If
        Dim dbQuery = TryCast(source, DbQuery(Of TSource))
        If dbQuery IsNot Nothing Then
            Return dbQuery.Include(path)
        End If
        Dim dbRepo = TryCast(source, IDbRepository(Of TSource))
        If dbRepo IsNot Nothing Then
            Return dbRepo.Include(path)
        End If
        Debug.Print("The source cannot be included")
        Return source
    End Function

End Module
