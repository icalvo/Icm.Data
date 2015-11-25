Imports System.Data.Entity
Imports Icm.Reflection
Imports System.Data.Entity.Infrastructure
Imports System.Linq.Expressions
Imports Proes.Data

Public Class DbRepository(Of TType As {TSet, Class}, TSet As Class, TKey As Structure)
    Implements IDbRepository(Of TType, TKey)

    Private ReadOnly _context As DbContext
    Private ReadOnly _idFunction As Func(Of Object, TKey)

    Protected Sub New(ByVal context As DbContext)
        _context = context
        _idFunction = Function(entity) DirectCast(ObjectReflectionExtensions.GetProp(entity, "id"), TKey)
    End Sub

    Protected ReadOnly Property DbSet As DbSet(Of TSet)
        Get
            Return Context.Set(Of TSet)()
        End Get
    End Property

    Public ReadOnly Property Context As DbContext
        Get
            Return _context
        End Get
    End Property

    Public Overridable Function Create() As TType Implements IDbRepository(Of TType, TKey).Create
        Return DbSet.Create(Of TType)()
    End Function

    Public Overridable Function Copy(other As TType) As TType Implements IDbRepository(Of TType, TKey).Copy
        Dim newObject = Create()
        other.CopyEntityTo(newObject, "id")
        Add(newObject)

        Return newObject
    End Function

    Protected Function TryCache(id As TKey) As TType
        Dim entity = Context _
           .ChangeTracker _
           .Entries _
           .SingleOrDefault(Function(entry) entry.State = EntityState.Unchanged AndAlso _idFunction(entry.Entity).Equals(id))

        If entity Is Nothing Then
            Return Nothing
        Else
            Return DirectCast(entity.Entity, TType)
        End If
    End Function

    Public Function GetById(ByVal id As Nullable2(Of TKey)) As TType Implements IDbRepository(Of TType, TKey).GetById
        If id.HasValue Then
            Return TryCast(DbSet.Find(id.Value), TType)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetByIdOrCreate(ByVal id As Nullable2(Of TKey), Optional bypassCache As Boolean = False, Optional ByVal includePath As String = Nothing) As TType Implements IDbRepository(Of TType, TKey).GetByIdOrCreate
        Dim obj = GetById(id)
        If obj Is Nothing Then
            obj = Create()
        End If
        Return obj
    End Function

    Public Sub Add(ByVal entity As TType) Implements IEntityRepository(Of TType).Add
        DbSet.Add(entity)
    End Sub

    Public Function DeleteById(ByVal id As TKey) As TType Implements IDbRepository(Of TType, TKey).DeleteById
        Dim result = GetById(id)
        DbSet.Remove(result)

        Return result
    End Function

    Public Sub Delete(ByVal entity As TType) Implements IEntityRepository(Of TType).Delete
        DbSet.Remove(entity)
    End Sub

    Public Function Include(ParamArray includeExpressions As Expression(Of Func(Of TType, Object))()) As IQueryable(Of TType) Implements IDbRepository(Of TType, TKey).Include
        Dim query = DbSet.AsQueryable.OfType(Of TType)()
        For Each includeExpression In includeExpressions
            query = query.Include(includeExpression)
        Next
        Return query
    End Function

    Public Function Include(ParamArray paths As String()) As IEnumerable(Of TType) Implements IDbRepository(Of TType, TKey).Include
        Dim query = DbSet.AsQueryable
        For Each path In paths
            query = query.Include(path)
        Next
        Return query.OfType(Of TType)()
    End Function

    Public Function Local() As IEnumerable(Of TType) Implements IDbRepository(Of TType).Local
        Return DbSet.Local.OfType(Of TType)()
    End Function

    Public Sub Save() Implements IEntityRepository(Of TType).Save
        Context.SaveChanges()
    End Sub

    Private Function GetEnumerator() As IEnumerator(Of TType) Implements IEnumerable(Of TType).GetEnumerator
        Return DbSet.OfType(Of TType).GetEnumerator
    End Function

    Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DbSet.OfType(Of TType).GetEnumerator
    End Function

    Private ReadOnly Property ElementType As Type Implements IQueryable.ElementType
        Get
            Return DirectCast(DbSet, IQueryable(Of TType)).ElementType
        End Get
    End Property

    Private ReadOnly Property Expression As Expression Implements IQueryable.Expression
        Get
            Return DirectCast(DbSet, IQueryable(Of TType)).Expression
        End Get
    End Property

    Private ReadOnly Property Provider As IQueryProvider Implements IQueryable.Provider
        Get
            Return DirectCast(DbSet, IQueryable(Of TType)).Provider
        End Get
    End Property
End Class

Public Class DbRepository(Of TType As {TSet, Class}, TSet As Class)
    Inherits DbRepository(Of TType, TSet, Integer)

    Public Sub New(ctx As DbContext)
        MyBase.New(ctx)
    End Sub
End Class

Public Class DbRepository(Of TSet As Class)
    Inherits DbRepository(Of TSet, TSet, Integer)

    Public Sub New(ByVal context As DbContext)
        MyBase.New(context)
    End Sub

End Class