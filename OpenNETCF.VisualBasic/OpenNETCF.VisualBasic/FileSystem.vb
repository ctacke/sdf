#Region "--- Copyright Information ---"
' *******************************************************************
'|                                                                   |
'|           OpenNETCF Smart Device Framework 2.2                    |
'|                                                                   |
'|                                                                   |
'|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
'|       ALL RIGHTS RESERVED                                         |
'|                                                                   |
'|   The entire contents of this file is protected by U.S. and       |
'|   International Copyright Laws. Unauthorized reproduction,        |
'|   reverse-engineering, and distribution of all or any portion of  |
'|   the code contained in this file is strictly prohibited and may  |
'|   result in severe civil and criminal penalties and will be       |
'|   prosecuted to the maximum extent possible under the law.        |
'|                                                                   |
'|   RESTRICTIONS                                                    |
'|                                                                   |
'|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
'|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
'|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
'|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
'|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
'|                                                                   |
'|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
'|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
'|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
'|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
'|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
'|                                                                   |
'|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
'|   ADDITIONAL RESTRICTIONS.                                        |
'|                                                                   |
' ******************************************************************* 
#End Region



Imports System.IO

''' <summary>
''' The FileSystem module contains the procedures used to perform file, directory or folder, and system operations.
''' </summary>
''' <history>
''' 	[Peter]	13/04/2004	Created
''' </history>
Public Module FileSystem

    ''' <summary>
    ''' Copies a file.
    ''' </summary>
    ''' <param name="Source">Required. String expression that specifies the name of the file to be copied. Source may include the directory or folder, and drive, of the source file.</param>
    ''' <param name="Destination">Required. String expression that specifies the target file name. Destination may include the directory or folder, and drive, of the destination file.</param>
    ''' <remarks>
    ''' If you try to use the FileCopy function on a currently open file, an error occurs.
    ''' </remarks>
    ''' <history>
    ''' 	[Peter]	13/04/2004	Created
    ''' </history>
    ''' <example>This example uses the <b>FileCopy</b> function to copy one file to another.
    ''' For purposes of this example, assume that SRCFILE is a file containing some data.
    ''' <code>[VB]
    ''' Dim SourceFile, DestinationFile As String
    ''' SourceFile = "SRCFILE"   ' Define source file name.
    ''' DestinationFile = "DESTFILE"   ' Define target file name.
    ''' FileCopy(SourceFile, DestinationFile)   ' Copy source to target.
    ''' </code></example>
    Public Sub FileCopy(ByVal Source As String, ByVal Destination As String)

        Try
            System.IO.File.Copy(Source, Destination)
        Catch arg As ArgumentException
            Err.Raise(52, Nothing, "Source or Destination is invalid or not specified.")
        Catch fnf As FileNotFoundException
            Err.Raise(53, Nothing, "File does not exist.")
        Catch io As IOException
            Err.Raise(55, Nothing, "File is already open.")
        End Try

    End Sub

    ''' <summary>
    ''' Deletes files from a disk.
    ''' </summary>
    ''' <param name="PathName">Required. String expression that specifies a file name to be deleted.</param>
    ''' <history>
    ''' 	[Peter]	13/04/2004	Created
    ''' </history>
    ''' <example>This example uses the <b>Kill</b> function to delete a file from a disk.
    ''' <code>[VB]
    ''' ' Assume TESTFILE is a file containing some data.
    ''' Kill("TestFile")   ' Delete file.
    ''' </code>
    ''' </example>
    Public Sub Kill(ByVal PathName As String)

        Try
            System.IO.File.Delete(PathName)
        Catch fnf As FileNotFoundException
            Err.Raise(53, Nothing, "Target file not found.")
        Catch io As IOException
            Err.Raise(55, Nothing, "Target file is open.")
        End Try

    End Sub

    ''' <summary>
    ''' Returns a Date value that indicates the date and time when a file was created or last modified.
    ''' </summary>
    ''' <param name="PathName">Required. String expression that specifies a file name. PathName may include the directory or folder, and the drive.</param>
    ''' <returns>The date and time when a file was created or last modified.</returns>
    ''' <history>
    ''' 	[Peter]	13/04/2004	Created
    ''' </history>
    ''' <example>This example uses the FileDateTime function to determine the date and time a file was created or last modified.
    ''' The format of the date and time displayed is based on the locale settings of your system.
    ''' <code>[VB]
    ''' Dim MyStamp As Date
    ''' ' Assume TESTFILE was last modified on October 12, 2001 at 4:35:47 PM.
    ''' ' Assume English/U.S. locale settings.
    ''' ' Returns "10/12/2001 4:35:47 PM".
    ''' MyStamp = FileDateTime("\TESTFILE.txt")
    ''' </code>
    ''' </example>
    Public Function FileDateTime(ByVal PathName As String) As Date

        Try
            Dim fi As New System.IO.FileInfo(PathName)
            FileDateTime = fi.CreationTime
        Catch fnf As FileNotFoundException
            Err.Raise(53, Nothing, "File does not exist")
        End Try

    End Function


    ''' <summary>
    ''' Returns a Long value specifying the length of a file in bytes.
    ''' </summary>
    ''' <param name="PathName">String expression that specifies a file. Pathname may include the directory or folder, and the drive.</param>
    ''' <returns>The length of a file in bytes.</returns>
    ''' <remarks>
    ''' If the specified file is open when the FileLen function is called, the value returned represents the size of the file at the time it was opened.
    ''' </remarks>
    ''' <history>
    ''' 	[Peter]	13/04/2004	Created
    ''' </history>
    ''' <example>This example uses the FileLen function to return the length of a file in bytes.
    ''' For purposes of this example, assume that test.txt is a file containing some data.
    ''' <code>[VB]
    ''' Dim MySize As Long
    ''' MySize = FileLen("\test.txt")   ' Returns file length (bytes).
    ''' </code>
    ''' </example>
    Public Function FileLen(ByVal PathName As String) As Long

        Try
            Dim fi As New System.IO.FileInfo(PathName)
            FileLen = fi.Length
        Catch fnf As FileNotFoundException
            Err.Raise(53, Nothing, "File does not exist")
        End Try

    End Function

    ''' <summary>
    ''' Renames a disk file, directory, or folder.
    ''' </summary>
    ''' <param name="OldPath">Required. String expression that specifies the existing file name and location. OldPath may include the directory or folder, and drive, of the file.</param>
    ''' <param name="NewPath">Required. String expression that specifies the new file name and location. NewPath may include directory or folder, and drive of the destination location. The file name specified by NewPath can't already exist.</param>
    ''' <remarks>
    ''' The Rename function renames a file and moves it to a different directory or folder, if necessary.
    ''' The Rename function can move a file across drives, but it can only rename an existing directory or folder when both NewPath and OldPath are located on the same drive.
    ''' Name cannot create a new file, directory, or folder.
    ''' </remarks>
    ''' <history>
    ''' 	[Peter]	13/04/2004	Created
    ''' </history>
    ''' <example>This example uses the Rename function to rename a file.
    ''' For purposes of this example, assume that the directories or folders that are specified already exist.
    ''' <code>[VB]
    ''' Dim OldName, NewName As String
    ''' OldName = "OLDFILE"
    ''' NewName = "NEWFILE" ' Define file names.
    ''' Rename(OldName, NewName)   ' Rename file.
    ''' 
    ''' OldName = "\MYDIR\OLDFILE"
    ''' NewName = "\YOURDIR\NEWFILE"
    ''' Rename(OldName, NewName)   ' Move and rename file.
    ''' </code>
    ''' </example>
    Public Sub Rename(ByVal OldPath As String, ByVal NewPath As String)

        Try
            System.IO.File.Move(OldPath, NewPath)
        Catch arg As ArgumentException
            Err.Raise(5, Nothing, "Pathname is invalid")
        Catch fnf As FileNotFoundException
            Err.Raise(53, Nothing, "OldPath file does not exist")
        Catch io As IOException
            Err.Raise(58, Nothing, "NewPath file already exists")
        End Try

    End Sub

    ''' <summary>
    ''' Removes an existing directory or folder.
    ''' </summary>
    ''' <param name="Path">Required.
    ''' <see cref="String"/> expression that identifies the directory or folder to be removed.</param>
    ''' <remarks>
    ''' An error occurs if you try to use RmDir on a directory or folder containing files.
    ''' Delete all files before attempting to remove a directory or folder.
    ''' </remarks>
    ''' <history>
    ''' 	[Peter]	03/05/2004	Created
    ''' </history>
    ''' <example>
    ''' This example uses the RmDir function to remove an existing directory or folder.
    ''' <code>[VB]
    ''' ' Assume that MYDIR is an empty directory or folder.
    ''' RmDir ("\MYDIR")   ' Remove MYDIR
    ''' </code>
    ''' </example>
    Public Sub RmDir(ByVal Path As String)

        Try
            System.IO.Directory.Delete(Path)
        Catch arg As ArgumentException
            Err.Raise(52, Nothing, "Path is not specified or is empty")
        Catch fnf As FileNotFoundException
            Err.Raise(76, Nothing, "Directory does not exist")
        Catch io As IOException
            Err.Raise(75, Nothing, "Target directory contains files.")
        End Try

    End Sub

End Module
