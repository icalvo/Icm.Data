

Public Interface IDataContext

    Function CreateObject(Of T)() As T

    Sub SaveChanges()

End Interface
