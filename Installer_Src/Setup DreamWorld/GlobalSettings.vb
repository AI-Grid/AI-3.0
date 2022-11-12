#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Module GlobalSettings

#Region "Const"

    Public Const _Domain As String = "http://www.outworldz.com"
    Public Const _httpsDomain As String = "https://www.outworldz.com"
    Public Const _MyVersion As String = "5.27"
    Public Const _SimVersion As String = "Opensim core #0069c9fef334b fix bullet config for mac osx"
    Public Const FreeDiskSpaceWarn As Long = 100000000  ' 100 MB to freeze
    Public Const JOpensim As String = "JOpensim"
    Public Const jOpensimRev As String = "Joomla_3.9.23-Stable-Full_Package"
    Public Const jRev As String = "3.9.23"
    Public Const MetroServer As String = "Metro"
    Public Const OsgridServer As String = "OsGrid"
    Public Const Outworldz As String = "Outworldz"
    Public Const RegionServerName As String = "Region"
    Public Const RobustServerName As String = "Robust"
    Public ApacheRevision As String = "2.4.52-64"
    Public AssemblyV As String
    Public MySqlRev As String = "5.6.50"

#End Region

    ' TODO
    'SELECT -- DISTINCT
    'parent1.folderName AS Parent, child1.type , child1.folderName AS Child,
    'child2.folderName AS GrandChild, child3.folderName AS GGChild , child4.folderName AS GGGChild,
    'child5.folderName AS GGGGChild, child6.folderName AS GGGGGChild , child7.folderName AS GGGGGGChild,
    'useraccounts.FirstName
    'FROM
    'useraccounts ,
    'inventoryfolders parent1
    'LEFT JOIN inventoryfolders child1 ON child1.parentFolderID = parent1.folderID
    'LEFT JOIN inventoryfolders child2 ON child2.parentFolderID = child1.folderID
    'LEFT JOIN inventoryfolders child3 ON child3.parentFolderID = child2.folderID
    'LEFT JOIN inventoryfolders child4 ON child4.parentFolderID = child3.folderID
    'LEFT JOIN inventoryfolders child5 ON child5.parentFolderID = child4.folderID
    'LEFT JOIN inventoryfolders child6 ON child6.parentFolderID = child5.folderID
    'LEFT JOIN inventoryfolders child7 ON child7.parentFolderID = child6.folderID
    'WHERE
    'parent1.agentID = useraccounts.PrincipalID
    'AND parent1.folderName = "My Inventory

End Module
