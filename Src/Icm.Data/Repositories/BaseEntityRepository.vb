Imports System.Data.Objects
Imports Proes.Data

Public MustInherit Class BaseEntityRepository(Of TType As {TSet, Class}, TSet As Class, TKey As Structure, TContext As IObjectContext)
    Implements IEntityRepository(Of TType, TKey)


    Private ReadOnly _context As TContext

    Protected Sub New(ByVal context As TContext)
        _context = context
    End Sub

    Protected MustOverride ReadOnly Property ObjectSets As ICollection(Of ObjectSet(Of TSet))

    Protected MustOverride ReadOnly Property MainObjectSet As ObjectSet(Of TSet)

    Protected ReadOnly Property Context As TContext
        Get
            Return _context
        End Get
    End Property

    Public Overridable Function Create() As TType Implements IEntityRepository(Of TType, TKey).Create
        Return Context.CreateObject(Of TType)()
    End Function

    Public Overridable Function Copy(other As TType) As TType Implements IEntityRepository(Of TType, TKey).Copy
        Dim newObject = Create()
        other.CopyEntityTo(newObject, "id")
        Add(newObject)
        Return newObject
    End Function

    Protected MustOverride Function GetQueryById(ByVal query As IQueryable(Of TType), ByVal id As TKey) As IQueryable(Of TType)

    Protected Function TryCache(id As TKey) As TType
        Dim osquery = Context _
           .ObjectStateManager _
           .GetObjectStateEntries(EntityState.Unchanged) _
           .Select(Function(ose) ose.Entity) _
           .OfType(Of TType) _
           .AsQueryable()

        Return GetQueryById(osquery, id).SingleOrDefault
    End Function

    Public Function GetById(ByVal id As TKey?, Optional bypassCache As Boolean = False, Optional ByVal includePath As String = Nothing) As TType Implements IEntityRepository(Of TType, TKey).GetById
        If id.HasValue Then
            If Not bypassCache Then
                Dim objCached = TryCache(id.Value)

                If objCached IsNot Nothing Then
                    Return objCached
                End If
            End If

            If includePath Is Nothing Then
                Return GetQueryById(MainObjectSet.OfType(Of TType), id.Value).SingleOrDefault()
            Else
                Return GetQueryById(MainObjectSet.OfType(Of TType), id.Value).Include(includePath).SingleOrDefault()
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Function GetByIdOrCreate(ByVal id As TKey?, Optional bypassCache As Boolean = False, Optional ByVal includePath As String = Nothing) As TType Implements IEntityRepository(Of TType, TKey).GetByIdOrCreate
        Dim obj = GetById(id, bypassCache, includePath)
        If obj Is Nothing Then
            obj = Create()
        End If
        Return obj
    End Function

    Public Sub Add(ByVal entity As TType) Implements IEntityRepository(Of TType, TKey).Add
        MainObjectSet.AddObject(entity)
    End Sub

    Public Function DeleteById(ByVal id As TKey) As TType Implements IEntityRepository(Of TType, TKey).DeleteById
        Dim result = GetById(id)
        MainObjectSet.DeleteObject(result)

        Return result
    End Function

    Public Sub Delete(ByVal entity As TType) Implements IEntityRepository(Of TType, TKey).Delete
        MainObjectSet.DeleteObject(entity)
    End Sub

    Private Function All() As IEnumerable(Of TType)
        Return MainObjectSet.OfType(Of TType)()
    End Function


    Public Sub Save() Implements IEntityRepository(Of TType, TKey).Save
        Context.SaveChanges()
    End Sub

    Private Function GetEnumerator() As IEnumerator(Of TType) Implements IEnumerable(Of TType).GetEnumerator
        Return All.GetEnumerator
    End Function

    Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return All.GetEnumerator
    End Function

End Class
