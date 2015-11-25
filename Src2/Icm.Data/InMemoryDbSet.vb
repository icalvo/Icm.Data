Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Collections
Imports System.Collections.ObjectModel

Namespace FakeDbSet
    ''' <summary>
    ''' The in-memory database set, taken from Microsoft's online example (http://msdn.microsoft.com/en-us/ff714955.aspx) 
    ''' and modified to be based on IDbSet instead of ObjectSet.
    ''' </summary>
    ''' <typeparam name="T">The type of entities in the DbSet.</typeparam>
    Public Class InMemoryDbSet(Of T As Class)
        Implements IDbSet(Of T)
        Private Shared ReadOnly _staticData As New HashSet(Of T)()
        Private ReadOnly _nonStaticData As HashSet(Of T)

        ''' <summary>
        ''' The non static backing store data for the InMemoryDbSet.
        ''' </summary>
        Private ReadOnly Property Data() As HashSet(Of T)
            Get
                If _nonStaticData Is Nothing Then
                    Return _staticData
                Else
                    Return _nonStaticData
                End If
            End Get
        End Property

        Public Property FindFunction As Func(Of IEnumerable(Of T), Object(), T)

        ''' <summary>
        ''' Creates an instance of the InMemoryDbSet using the default static backing store.This means
        ''' that data persists between test runs, like it would do with a database unless you
        ''' cleared it down.
        ''' </summary>
        Public Sub New()
            Me.New(True)
        End Sub

        ''' <summary>
        ''' This constructor allows you to pass in your own data store, instead of using
        ''' the static backing store.
        ''' </summary>
        ''' <param name="data">A place to store data.</param>
        Public Sub New(data As HashSet(Of T))
            _nonStaticData = data
        End Sub

        ''' <summary>
        ''' Creates an instance of the InMemoryDbSet using the default static backing store.This means
        ''' that data persists between test runs, like it would do with a database unless you
        ''' cleared it down.
        ''' </summary>
        ''' <param name="clearDownExistingData"></param>
        Public Sub New(clearDownExistingData As Boolean)
            If clearDownExistingData Then
                Clear()
            End If
        End Sub

        Public Sub Clear()
            Data.Clear()
        End Sub

        Public Function Add(entity As T) As T Implements IDbSet(Of T).Add
            Data.Add(entity)

            Return entity
        End Function

        Public Function Attach(entity As T) As T Implements IDbSet(Of T).Attach
            Data.Add(entity)
            Return entity
        End Function

        Public Function Create(Of TDerivedEntity As {Class, T})() As TDerivedEntity Implements IDbSet(Of T).Create
            Return Activator.CreateInstance(Of TDerivedEntity)()
        End Function

        Public Function Create() As T Implements IDbSet(Of T).Create
            Return Activator.CreateInstance(Of T)()
        End Function

        Public Overridable Function Find(ParamArray keyValues As Object()) As T Implements IDbSet(Of T).Find
            If FindFunction Is Nothing Then
                Return FindFunction(Data, keyValues)
            Else
                Throw New NotImplementedException("Derive from InMemoryDbSet and override Find, or provide a FindFunction.")
            End If
        End Function

        Public ReadOnly Property Local() As ObservableCollection(Of T) Implements IDbSet(Of T).Local
            Get
                Return New ObservableCollection(Of T)(Data)
            End Get
        End Property

        Public Function Remove(entity As T) As T Implements IDbSet(Of T).Remove
            Data.Remove(entity)

            Return entity
        End Function

        Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Return Data.GetEnumerator()
        End Function

        Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Data.GetEnumerator()
        End Function

        Public ReadOnly Property ElementType() As Type Implements IQueryable(Of T).ElementType
            Get
                Return Data.AsQueryable.ElementType
            End Get
        End Property

        Public ReadOnly Property Expression() As Expression Implements IQueryable(Of T).Expression
            Get
                Return Data.AsQueryable.Expression
            End Get
        End Property

        Public ReadOnly Property Provider() As IQueryProvider Implements IQueryable(Of T).Provider
            Get
                Return Data.AsQueryable.Provider
            End Get
        End Property
    End Class
End Namespace