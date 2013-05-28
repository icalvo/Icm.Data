Imports System.Data.Objects
Imports System.Data.Common
Imports System.Data.Metadata.Edm
Imports System.Runtime.InteropServices
Imports System.Data.Objects.DataClasses
Imports System.Linq.Expressions
Imports System.Runtime


Public Interface IObjectContext
    Inherits IDisposable

    ' Events
    Event ObjectMaterialized As ObjectMaterializedEventHandler
    Event SavingChanges As EventHandler

    ' Methods
    Sub AcceptAllChanges()
    Sub AddObject(ByVal entitySetName As String, ByVal entity As Object)
    Function ApplyCurrentValues(Of TEntity As Class)(ByVal entitySetName As String, ByVal currentEntity As TEntity) As TEntity
    Function ApplyOriginalValues(Of TEntity As Class)(ByVal entitySetName As String, ByVal originalEntity As TEntity) As TEntity

    Sub Attach(ByVal entity As IEntityWithKey)
    Sub AttachTo(ByVal entitySetName As String, ByVal entity As Object)
    Sub CreateDatabase()
    Function CreateDatabaseScript() As String
    Function CreateEntityKey(ByVal entitySetName As String, ByVal entity As Object) As EntityKey
    Function CreateObject(Of T As Class)() As T
    Function CreateObjectSet(Of TEntity As Class)() As ObjectSet(Of TEntity)
    Function CreateObjectSet(Of TEntity As Class)(ByVal entitySetName As String) As ObjectSet(Of TEntity)
    Sub CreateProxyTypes(ByVal types As IEnumerable(Of Type))
    Function CreateQuery(Of T)(ByVal queryString As String, ByVal ParamArray parameters As ObjectParameter()) As ObjectQuery(Of T)
    Function DatabaseExists() As Boolean
    Sub DeleteDatabase()
    Sub DeleteObject(ByVal entity As Object)
    Sub Detach(ByVal entity As Object)
    Sub DetectChanges()
    <TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")> _
    Function ExecuteFunction(Of TElement)(ByVal functionName As String, ByVal ParamArray parameters As ObjectParameter()) As ObjectResult(Of TElement)
    Function ExecuteFunction(ByVal functionName As String, ByVal ParamArray parameters As ObjectParameter()) As Integer
    Function ExecuteFunction(Of TElement)(ByVal functionName As String, ByVal mergeOption As MergeOption, ByVal ParamArray parameters As ObjectParameter()) As ObjectResult(Of TElement)
    Function ExecuteStoreCommand(ByVal commandText As String, ByVal ParamArray parameters As Object()) As Integer
    <TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")> _
    Function ExecuteStoreQuery(Of TElement)(ByVal commandText As String, ByVal ParamArray parameters As Object()) As ObjectResult(Of TElement)
    Function ExecuteStoreQuery(Of TEntity)(ByVal commandText As String, ByVal entitySetName As String, ByVal mergeOption As MergeOption, ByVal ParamArray parameters As Object()) As ObjectResult(Of TEntity)
    Function GetObjectByKey(ByVal key As EntityKey) As Object
    Sub LoadProperty(ByVal entity As Object, ByVal navigationProperty As String)
    Sub LoadProperty(Of TEntity)(ByVal entity As TEntity, ByVal selector As Expression(Of Func(Of TEntity, Object)))
    Sub LoadProperty(ByVal entity As Object, ByVal navigationProperty As String, ByVal mergeOption As MergeOption)
    Sub LoadProperty(Of TEntity)(ByVal entity As TEntity, ByVal selector As Expression(Of Func(Of TEntity, Object)), ByVal mergeOption As MergeOption)
    Sub Refresh(ByVal refreshMode As RefreshMode, ByVal collection As IEnumerable)
    Sub Refresh(ByVal refreshMode As RefreshMode, ByVal entity As Object)
    <TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")> _
    Function SaveChanges() As Integer
    Function Translate(Of TElement)(ByVal reader As DbDataReader) As ObjectResult(Of TElement)
    Function Translate(Of TEntity)(ByVal reader As DbDataReader, ByVal entitySetName As String, ByVal mergeOption As MergeOption) As ObjectResult(Of TEntity)
    Function TryGetObjectByKey(ByVal key As EntityKey, <Out()> ByRef value As Object) As Boolean

    ' Properties
    Property CommandTimeout As Nullable(Of Integer)
    ReadOnly Property Connection As DbConnection
    ReadOnly Property ContextOptions As ObjectContextOptions
    Property DefaultContainerName As String
    ReadOnly Property MetadataWorkspace As MetadataWorkspace
    ReadOnly Property ObjectStateManager As ObjectStateManager

End Interface
