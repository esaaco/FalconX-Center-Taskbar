﻿Imports System.Environment
Imports System.IO

Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Automation
Imports FalconX4.VisualEffects.Animations.Effects

Public Class Taskbar

    <DllImport("user32.dll", ExactSpelling:=True, CharSet:=CharSet.Auto)>
    Public Shared Function GetParent(ByVal hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="FindWindow", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function FindWindowByClass(ByVal lpClassName As String, ByVal zero As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Boolean, ByVal lParam As Int32) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SetWindowCompositionAttribute(ByVal hwnd As IntPtr, ByRef data As WindowCompositionAttributeData) As Integer
    End Function

    <DllImport("kernel32.dll")>
    Private Shared Function SetProcessWorkingSetSize(ByVal hProcess As IntPtr, ByVal dwMinimumWorkingSetSize As Int32, ByVal dwMaximumWorkingSetSize As Int32) As Int32
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInt32) As Boolean
    End Function

    <DllImport("SHCore.dll", SetLastError:=True)>
    Private Shared Function SetProcessDpiAwareness(ByVal awareness As PROCESS_DPI_AWARENESS) As Boolean
    End Function

    Private Enum PROCESS_DPI_AWARENESS
        Process_DPI_Unaware = 0
        Process_System_DPI_Aware = 1
        Process_Per_Monitor_DPI_Aware = 2
    End Enum

    Public Shared SWP_NOSIZE As UInt32 = 1
    Public Shared SWP_NOMOVE As UInt32 = 2
    Public Shared SWP_ASYNCWINDOWPOS As UInt32 = 16384
    Public Shared SWP_NOACTIVATE As UInt32 = 16
    Public Shared SWP_NOSENDCHANGING As UInt32 = 1024
    Public Shared SWP_NOZORDER As UInt32 = 4
    Public Shared WM_DWMCOLORIZATIONCOLORCHANGED As Integer = &H320
    Public Shared WM_DWMCOMPOSITIONCHANGED As Integer = &H31E
    Public Shared WM_THEMECHANGED As Integer = &H31A
    Public Shared WM_STYLECHANGED As Integer = &H7D
    Public Shared WM_SIZING As Integer = &H214
    Public Shared WM_WINDOWPOSCHANGED As Integer = &H47
    Public Shared WM_ERASEBKGND As Integer = &H14
    Public Shared WM_ENTERSIZEMOVE = &H231
    Public Shared WM_EXITSIZEMOVE = &H232
    Public Shared WM_COPYDATA = &H4A
    Public Shared WM_SETTINGCHANGE = &H1A
    Public Shared WM_USER = &H400
    Public Shared WM_SYSCOMMAND = &H112
    Public Shared WM_SETICON As UInteger = &H80
    Public Shared ICON_SMALL As Integer = 0
    Public Shared ICON_BIG As Integer = 1
    Public Shared WM_SIZE = &H5

    Friend Structure WindowCompositionAttributeData
        Public Attribute As WindowCompositionAttribute
        Public Data As IntPtr
        Public SizeOfData As Integer
    End Structure

    Friend Enum WindowCompositionAttribute
        WCA_ACCENT_POLICY = 19
    End Enum

    Friend Enum AccentState
        ACCENT_DISABLED = 0
        ACCENT_ENABLE_GRADIENT = 1
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2
        ACCENT_ENABLE_BLURBEHIND = 3
        ACCENT_ENABLE_TRANSPARANT = 6
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4
        ACCENT_NORMAL = 150
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure AccentPolicy
        Public AccentState As AccentState
        Public AccentFlags As Integer
        Public GradientColor As Integer
        Public AnimationId As Integer
    End Structure

    Public Shared Shell_TrayWnd As AutomationElement = AutomationElement.FromHandle(FindWindowByClass("Shell_TrayWnd", CType(0, IntPtr)))
    Public Shared MSTaskListWClass As AutomationElement = Shell_TrayWnd.FindFirst(TreeScope.Descendants, New PropertyCondition(AutomationElement.ClassNameProperty, "MSTaskListWClass"))
    Public Shared StartB As AutomationElement = Shell_TrayWnd.FindFirst(TreeScope.Descendants, New PropertyCondition(AutomationElement.ClassNameProperty, "Start"))
    Public Shared TrayNotifyWnd As AutomationElement = Shell_TrayWnd.FindFirst(TreeScope.Descendants, New PropertyCondition(AutomationElement.ClassNameProperty, "TrayNotifyWnd"))
    Public Shared MSTaskSwWClass As IntPtr = GetParent(CType(MSTaskListWClass.Current.NativeWindowHandle, IntPtr))
    Public Shared ReBarWindow32 As IntPtr = GetParent(MSTaskSwWClass)
    Public Shared Root As AutomationElement = AutomationElement.RootElement
    Public Shared Condition As New OrCondition(New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_TrayWnd"), New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_SecondaryTrayWnd"))
    Public Shared AllTrayWnds As AutomationElementCollection = Root.FindAll(TreeScope.Children, Condition)
    Public Shared Shell_TrayWndPtr As IntPtr = CType(Shell_TrayWnd.Current.NativeWindowHandle, IntPtr)
    Public Shared MSTaskListWClassPtr As IntPtr = CType(MSTaskListWClass.Current.NativeWindowHandle, IntPtr)
    Public Shared TrayNotifyWndPtr As IntPtr = CType(TrayNotifyWnd.Current.NativeWindowHandle, IntPtr)
    Public Shared MSTaskSwWClassPtr As IntPtr = MSTaskSwWClass
    Public Shared ReBarWindow32Ptr As IntPtr = ReBarWindow32
    Public Const WM_SETREDRAW As Integer = 11
    Public Shared Horizontal As Boolean
    Public Shared TaskbarTransparant As Boolean
    Public Shared CenterBetween As Boolean
    Public Shared UpdateTaskbar As Boolean
    Public Shared TaskbarChanged As Boolean
    Public Shared OffsetPosition As String
    Public Shared OffsetPosition2 As String
    Public Shared TaskbarStyle As Integer
    Public Shared UpdateTaskbarStyle As Boolean
    Public Shared MonitorChanged As Boolean
    Public Shared PrimaryTaskbarOnly As Boolean
    Public Shared Windows10Xmode As Boolean
    Public Shared Updating As Boolean
    Public Shared OldPosition1 As Integer
    Public Shared OldPosition2 As Integer
    Public Shared OldPosition3 As Integer
    Public Shared Orientation1 As Integer
    Public Shared Orientation2 As Integer
    Public Shared Orientation3 As Integer
    Public Shared OldLeft1 As Integer
    Public Shared OldLeft2 As Integer
    Public Shared OldLeft3 As Integer
    Public Shared YforHTaskbar As Integer
    Public Shared XforVTaskbar As Integer
    Public Shared Ready As Boolean
    Public Shared AppClosing As Boolean
    Public Shared RefreshRate As Integer

    Public Shared Sub LoadSettings()
        Try
            Dim path As String = GetFolderPath(SpecialFolder.ApplicationData) + "\FalconX.cfg"

            If File.Exists(path) Then
                AnimationControl.AnimationSelection = System.IO.File.ReadAllLines(path)(0)
                AnimationControl2.AnimationSelection = System.IO.File.ReadAllLines(path)(0)
                AnimationControl3.AnimationSelection = System.IO.File.ReadAllLines(path)(0)
                AnimationControl.AnimationSpeed = CDec(System.IO.File.ReadAllLines(path)(1))
                AnimationControl2.AnimationSpeed = CDec(System.IO.File.ReadAllLines(path)(1))
                AnimationControl3.AnimationSpeed = CDec(System.IO.File.ReadAllLines(path)(1))
                OffsetPosition = CDec(System.IO.File.ReadAllLines(path)(2))
                If System.IO.File.ReadAllLines(path)(3) = "True" Then
                Else
                End If
                If System.IO.File.ReadAllLines(path)(4) = "True" Then
                    CenterBetween = True
                Else
                    CenterBetween = False
                End If
                If System.IO.File.ReadAllLines(path)(5) = "True" Then
                    TaskbarTransparant = True
                Else
                    TaskbarTransparant = False
                End If
                If System.IO.File.ReadAllLines(path)(6) = "1" Then
                    TaskbarStyle = 1
                End If
                If System.IO.File.ReadAllLines(path)(6) = "2" Then
                    TaskbarStyle = 2
                End If
                If System.IO.File.ReadAllLines(path)(6) = "3" Then
                    TaskbarStyle = 3
                End If
                OffsetPosition2 = CDec(System.IO.File.ReadAllLines(path)(7))
                If System.IO.File.ReadAllLines(path)(8) = "True" Then
                    PrimaryTaskbarOnly = True
                Else
                    PrimaryTaskbarOnly = False
                End If
                XforVTaskbar = CDec(System.IO.File.ReadAllLines(path)(9))
                YforHTaskbar = CDec(System.IO.File.ReadAllLines(path)(9))

                RefreshRate = CDec(System.IO.File.ReadAllLines(path)(10))
            Else

                AnimationControl.AnimationSelection = "CubicEaseInOut"
                AnimationControl2.AnimationSelection = "CubicEaseInOut"
                AnimationControl3.AnimationSelection = "CubicEaseInOut"
                AnimationControl.AnimationSpeed = 500
                AnimationControl2.AnimationSpeed = 500
                AnimationControl3.AnimationSpeed = 500
                OffsetPosition = 0

                CenterBetween = False

                TaskbarTransparant = False

                TaskbarStyle = 1

                OffsetPosition2 = 0

                PrimaryTaskbarOnly = False

                XforVTaskbar = 0
                YforHTaskbar = 0

                RefreshRate = 400

                '  SaveSettings()
                '  LoadSettings()
            End If
        Catch 'ex As Exception
            '  MessageBox.Show("!!" & ex.Message)
        End Try
    End Sub

    Public Shared Sub SaveSettings()
        Try
            Dim path As String = GetFolderPath(SpecialFolder.ApplicationData) + "\FalconX.cfg"
            Dim fs As FileStream = File.Create(path)
            Dim Animation = "CubicEaseInOut"
            Dim Speed = "500"
            Dim Offset = "0"
            Dim RunAtStartUp = "False"
            Dim Transparant = "False"
            Dim CBT = "False"
            Dim TaskbarStyle = "1"
            Dim Offset2 = "0"
            Dim XforXTaskbar = "0"
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(Animation.ToString & Environment.NewLine & Speed.ToString & Environment.NewLine & Offset.ToString & Environment.NewLine & RunAtStartUp.ToString & Environment.NewLine & CBT.ToString & Environment.NewLine & Transparant.ToString & Environment.NewLine & TaskbarStyle.ToString & Environment.NewLine & Offset2.ToString & Environment.NewLine & XforXTaskbar.ToString)
            fs.Write(info, 0, info.Length)
            fs.Close()
        Catch
        End Try
    End Sub

    Const WS_BORDER = &H800000
    Const WS_DLGFRAME = &H400000
    Const WS_THICKFRAME = &H40000
    Const WS_CAPTION = &HC00000
    Const WS_EX_CLIENTEDGE = &H200
    Const WS_EX_LEFT = &H0&
    Const HWND_BOTTOM = 1
    Const HWND_TOP = 0
    Const HWND_TOPMOST = -1
    Const HWND_NOTOPMOST = -2
    Const SWP_SHOWWINDOW = &H40

    Public Structure RECT
        Public Left As Long
        Public Top As Long
        Public Right As Long
        Public Bottom As Long
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure CopyData
        Public dwData As IntPtr
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure

    <Flags()>
    Public Enum SendMessageTimeoutFlags
        SMTO_NORMAL = 0
        SMTO_BLOCK = 1
        SMTO_ABORTIFHUNG = 2
        SMTO_NOTIMEOUTIFNOTHUNG = 8
    End Enum

    Public Shared Sub Main()
        SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware)
        Try
            For Each prog As Process In Process.GetProcesses
                If prog.ProcessName = "FalconX" Then
                    If Not prog.Id = Process.GetCurrentProcess.Id Then
                        prog.Kill()
                    End If
                End If
            Next
        Catch
        End Try
        Dim Handle As IntPtr
        Do
            Console.WriteLine("Waiting for Shell_TrayWnd")
            Handle = Nothing
            System.Threading.Thread.Sleep(250)
            Handle = FindWindowByClass("Shell_TrayWnd", CType(0, IntPtr))
        Loop Until Not Handle = Nothing
        ResetTaskbarStyle()
        UpdateTaskbar = True
        RefreshWindowsExplorer()
        Dim currentProcess As Process = Process.GetCurrentProcess
        currentProcess.PriorityClass = ProcessPriorityClass.High
        LoadSettings()
        System.Threading.Thread.Sleep(500)
        If TaskbarTransparant = True Then
            Dim t2 As System.Threading.Thread = New System.Threading.Thread(AddressOf EnableTaskbarStyle)
            t2.Start()
        End If
        SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, False, 0)
        SendMessage(GetParent(Shell_TrayWndPtr), WM_SETREDRAW, False, 0)
        Dim t1 As System.Threading.Thread = New System.Threading.Thread(AddressOf TaskbarCalculator)
        t1.Start()
    End Sub

    Public Shared Sub TaskbarCalculator()
        Dim TaskbarTreeWalker As TreeWalker = TreeWalker.ControlViewWalker
        Dim Laps As Integer
        Do
            Try
                Updating = True
                If AppClosing = True Then
                    Exit Sub
                End If
                Dim TaskbarCount As Integer = 0
                Dim OldTrayNotifyWidth As Integer
                Dim TrayNotifyWidth As Integer = 0
                Dim Resolution As Integer
                Dim OldResolution As Integer
                Dim Monitors As Integer
                Dim OldMonitors As Integer
                Dim OldTaskbarCount As Integer
                Dim child As AutomationElement = TaskbarTreeWalker.GetLastChild(MSTaskListWClass)
                If Shell_TrayWnd.Current.BoundingRectangle.Height >= 200 Then
                    If Horizontal = True Then
                        UpdateTaskbar = True
                        Console.WriteLine("Taskbar Position Changed")
                    End If
                    Horizontal = False
                Else
                    If Horizontal = False Then
                        UpdateTaskbar = True
                        Console.WriteLine("Taskbar Position Changed")
                    End If
                    Horizontal = True
                End If
                Dim screencount As Integer
                screencount = 0
                For Each screenX As Screen In Screen.AllScreens
                    screencount = screencount + screenX.Bounds.Width
                Next
                Resolution = screencount
                If Horizontal = True Then
                    TaskbarCount = CInt(child.Current.BoundingRectangle.Left)
                    TrayNotifyWidth = CInt(TrayNotifyWnd.Current.BoundingRectangle.Left)
                Else
                    TaskbarCount = CInt(child.Current.BoundingRectangle.Top)
                    TrayNotifyWidth = CInt(TrayNotifyWnd.Current.BoundingRectangle.Top)
                End If
                SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, False, 0)
                SendMessage(GetParent(Shell_TrayWndPtr), WM_SETREDRAW, False, 0)
                If Not OldResolution = Resolution Then
                    If Not OldResolution = 0 Then
                        Console.WriteLine("Resolution Changed.")
                        Taskbar.RefreshWindowsExplorer()
                        System.Threading.Thread.Sleep(500)
                        RootX = AutomationElement.RootElement
                        Dim ConditionX As New OrCondition(New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_TrayWnd"), New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_SecondaryTrayWnd"))
                        Dim AllTrayWndsX As AutomationElementCollection = Root.FindAll(TreeScope.Children, Condition)
                        AllTrayWnds = Root.FindAll(TreeScope.Children, Condition)
                        UpdateTaskbar = True
                        UpdateTaskbarStyle = True
                        TaskbarChanged = True
                        System.Threading.Thread.Sleep(500)
                        If Orientation1 = True Then
                            SetWindowPos(XLocationEffect.FirstTaskbarPtr, IntPtr.Zero, XLocationEffect.FirstTaskbarPosition, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                        Else
                            SetWindowPos(XLocationEffect.FirstTaskbarPtr, IntPtr.Zero, 0, XLocationEffect.FirstTaskbarPosition, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                        End If
                        If Orientation2 = True Then
                            SetWindowPos(XLocationEffect2.SecondTaskbarPtr, IntPtr.Zero, XLocationEffect2.SecondTaskbarPosition, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                        Else
                            SetWindowPos(XLocationEffect2.SecondTaskbarPtr, IntPtr.Zero, 0, XLocationEffect2.SecondTaskbarPosition, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                        End If
                        If Orientation3 = True Then
                            SetWindowPos(XLocationEffect3.ThirdTaskbarPtr, IntPtr.Zero, XLocationEffect3.ThirdTaskbarPosition, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                        Else
                            SetWindowPos(XLocationEffect3.ThirdTaskbarPtr, IntPtr.Zero, 0, XLocationEffect3.ThirdTaskbarPosition, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                        End If
                        Console.WriteLine(OldResolution & "|" & Resolution)
                    End If
                End If
                If Not TrayNotifyWidth = OldTrayNotifyWidth Then
                    If Not OldTrayNotifyWidth = 0 Then
                        If Not MSTaskListWClass.Current.BoundingRectangle.X = 0 Then
                            If TrayNotifyWnd.Current.BoundingRectangle.Left = 3 Then
                                Closing()
                                Exit Sub
                            End If
                            Dim rebar As AutomationElement = AutomationElement.FromHandle(GetParent(CType(MSTaskListWClassPtr, IntPtr)))
                            Dim offset = CInt(rebar.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                            Dim pos = MSTaskListWClass.Current.BoundingRectangle.X - offset
                            SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, True, 0)
                            SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, False, 0)
                            If Taskbar.Horizontal = True Then
                                SetWindowPos(MSTaskListWClassPtr, IntPtr.Zero, pos, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                            Else
                                SetWindowPos(MSTaskListWClassPtr, IntPtr.Zero, 0, pos, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
                            End If
                            TaskbarChanged = True
                        End If
                    End If
                End If
                System.Threading.Thread.Sleep(RefreshRate)
                OldResolution = Resolution
                OldTrayNotifyWidth = TrayNotifyWidth
                If Not TaskbarCount = OldTaskbarCount Or UpdateTaskbar = True Or Not Resolution = OldResolution Then
                    If Not TaskbarCount = OldTaskbarCount + 1 Or Not TrayNotifyWidth = OldTrayNotifyWidth Then
                        If Not TaskbarCount = OldTaskbarCount - 1 Or Not TrayNotifyWidth = OldTrayNotifyWidth Then
                            If Not TaskbarCount = OldTaskbarCount + 2 Or Not TrayNotifyWidth = OldTrayNotifyWidth Then
                                If Not TaskbarCount = OldTaskbarCount - 2 Or Not TrayNotifyWidth = OldTrayNotifyWidth Then
                                    If Not TaskbarCount = OldTaskbarCount + 3 Or Not TrayNotifyWidth = OldTrayNotifyWidth Then
                                        If Not TaskbarCount = OldTaskbarCount - 3 Or Not TrayNotifyWidth = OldTrayNotifyWidth Then
                                            Laps = 0
                                            Console.WriteLine("")
                                            Console.WriteLine("The Taskbar Watcher found a difference")
                                            Console.WriteLine("TaskbarCount = " & TaskbarCount & " | OldTaskbarCount = " & OldTaskbarCount)
                                            Console.WriteLine("Resolution = " & Resolution & " | OldResolution = " & OldResolution)
                                            Console.WriteLine("")
                                            System.Threading.Thread.Sleep(RefreshRate)
                                            If Not Resolution = OldResolution Then
                                                TaskbarChanged = True
                                            End If
                                            If Not TrayNotifyWidth = OldTrayNotifyWidth Then
                                                TaskbarChanged = True
                                            End If
                                            Dim TaskListID As Integer = 0
                                            Console.WriteLine("UpdateTaskbar = " & UpdateTaskbar)
                                            For Each trayWnd As AutomationElement In AllTrayWnds
                                                TaskListID = TaskListID + 1
                                                Dim TaskList As AutomationElement = trayWnd.FindFirst(TreeScope.Descendants, New PropertyCondition(AutomationElement.ClassNameProperty, "MSTaskListWClass"))
                                                Dim TrayNotify As AutomationElement = trayWnd.FindFirst(TreeScope.Descendants, New PropertyCondition(AutomationElement.ClassNameProperty, "TrayNotifyWnd"))
                                                OldTaskbarCount = TaskbarCount
                                                If TaskListID = 1 Then
                                                    If PrimaryTaskbarOnly = True Then
                                                        If Not trayWnd.Current.ClassName = "Shell_TrayWnd" Then
                                                            Continue For
                                                        End If
                                                    End If
                                                    Dim TreeWalker1 As TreeWalker = TreeWalker.ControlViewWalker
                                                    Dim BChildFirst1 As AutomationElement = TreeWalker1.GetFirstChild(TaskList)
                                                    Dim BChildLast1 As AutomationElement = TreeWalker1.GetLastChild(TaskList)
                                                    If TaskList.Current.BoundingRectangle.Height >= 200 Then
                                                        Orientation1 = False
                                                    Else
                                                        Orientation1 = True
                                                    End If
                                                    Dim TaskbarWidth1 As Integer
                                                    If Orientation1 = True Then
                                                        TaskbarWidth1 = CInt((BChildFirst1.Current.BoundingRectangle.Left - TaskList.Current.BoundingRectangle.Left) + (BChildLast1.Current.BoundingRectangle.Left - TaskList.Current.BoundingRectangle.Left) + BChildLast1.Current.BoundingRectangle.Width)
                                                    Else
                                                        TaskbarWidth1 = CInt((BChildFirst1.Current.BoundingRectangle.Top - TaskList.Current.BoundingRectangle.Top) + (BChildLast1.Current.BoundingRectangle.Top - TaskList.Current.BoundingRectangle.Top) + BChildLast1.Current.BoundingRectangle.Height)
                                                    End If
                                                    Dim RebarWnd1 As AutomationElement = AutomationElement.FromHandle(GetParent(CType(TaskList.Current.NativeWindowHandle, IntPtr)))
                                                    Dim TrayWndLeft1 As Integer
                                                    Dim TrayWndWidth1 As Integer
                                                    Dim RebarWndLeft1 As Integer
                                                    Dim TaskbarLeft1 As Integer
                                                    If Orientation1 = True Then
                                                        TrayWndLeft1 = CInt(trayWnd.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                                                        TrayWndWidth1 = CInt(trayWnd.Current.BoundingRectangle.Width.ToString.Replace("-", ""))
                                                        RebarWndLeft1 = CInt(RebarWnd1.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                                                        TaskbarLeft1 = CInt((RebarWndLeft1 - TrayWndLeft1).ToString.Replace("-", ""))
                                                    Else
                                                        TrayWndLeft1 = CInt(trayWnd.Current.BoundingRectangle.Top.ToString.Replace("-", ""))
                                                        TrayWndWidth1 = CInt(trayWnd.Current.BoundingRectangle.Height.ToString.Replace("-", ""))
                                                        RebarWndLeft1 = CInt(RebarWnd1.Current.BoundingRectangle.Top.ToString.Replace("-", ""))
                                                        TaskbarLeft1 = CInt((RebarWndLeft1 - TrayWndLeft1).ToString.Replace("-", ""))
                                                    End If
                                                    Dim Position1 As Integer
                                                    If trayWnd.Current.ClassName = "Shell_TrayWnd" Then
                                                        If CenterBetween = True Then
                                                            Dim offset = (TrayNotify.Current.BoundingRectangle.Width / 2 - (TaskbarLeft1 \ 2))
                                                            Position1 = CInt((TrayWndWidth1 / 2 - (TaskbarWidth1 / 2) - TaskbarLeft1 - offset).ToString.Replace("-", "")) + OffsetPosition
                                                        Else
                                                            Position1 = CInt((TrayWndWidth1 / 2 - (TaskbarWidth1 / 2) - TaskbarLeft1).ToString.Replace("-", "")) + OffsetPosition
                                                        End If
                                                    Else
                                                        Position1 = CInt((TrayWndWidth1 / 2 - (TaskbarWidth1 / 2) - TaskbarLeft1).ToString.Replace("-", "")) + OffsetPosition2
                                                    End If
                                                    XLocationEffect.FirstTaskbarPtr = CType(TaskList.Current.NativeWindowHandle, IntPtr)
                                                    XLocationEffect.FirstTaskbarPosition = CInt(Position1.ToString.Replace("-", ""))
                                                    XLocationEffect.FirstTaskbarOldPosition = CInt(OldPosition1.ToString.Replace("-", ""))
                                                    SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, False, 0)
                                                    SendMessage(GetParent(Shell_TrayWndPtr), WM_SETREDRAW, False, 0)
                                                    Console.WriteLine("FirstTaskbarCalculation | OldLeft = " & OldLeft1 & " Left = " & TaskbarLeft1 + TaskbarWidth1 & " <-- If not the same we call the Animator")
                                                    If Not OldLeft1 = TaskbarLeft1 + TaskbarWidth1 Or UpdateTaskbar = True Or TaskbarChanged = True Then
                                                        Console.WriteLine("Call Animator 1")
                                                        Dim t1 As System.Threading.Thread = New System.Threading.Thread(AddressOf AnimationControl.animateTaskbar)
                                                        t1.Start()
                                                    End If
                                                    OldPosition1 = Position1
                                                    OldLeft1 = TaskbarLeft1 + TaskbarWidth1
                                                End If
                                                If TaskListID = 2 Then
                                                    If PrimaryTaskbarOnly = True Then
                                                        If Not trayWnd.Current.ClassName = "Shell_TrayWnd" Then
                                                            Continue For
                                                        End If
                                                    End If
                                                    Dim TreeWalker2 As TreeWalker = TreeWalker.ControlViewWalker
                                                    Dim BChildFirst2 As AutomationElement = TreeWalker2.GetFirstChild(TaskList)
                                                    Dim BChildLast2 As AutomationElement = TreeWalker2.GetLastChild(TaskList)
                                                    If TaskList.Current.BoundingRectangle.Height >= 200 Then
                                                        Orientation2 = False
                                                    Else
                                                        Orientation2 = True
                                                    End If
                                                    Dim TaskbarWidth2 As Integer
                                                    If Orientation2 = True Then
                                                        TaskbarWidth2 = CInt((BChildFirst2.Current.BoundingRectangle.Left - TaskList.Current.BoundingRectangle.Left) + (BChildLast2.Current.BoundingRectangle.Left - TaskList.Current.BoundingRectangle.Left) + BChildLast2.Current.BoundingRectangle.Width)
                                                    Else
                                                        TaskbarWidth2 = CInt((BChildFirst2.Current.BoundingRectangle.Top - TaskList.Current.BoundingRectangle.Top) + (BChildLast2.Current.BoundingRectangle.Top - TaskList.Current.BoundingRectangle.Top) + BChildLast2.Current.BoundingRectangle.Height)
                                                    End If
                                                    Dim RebarWnd2 As AutomationElement = AutomationElement.FromHandle(GetParent(CType(TaskList.Current.NativeWindowHandle, IntPtr)))
                                                    Dim TrayWndLeft2 As Integer
                                                    Dim TrayWndWidth2 As Integer
                                                    Dim RebarWndLeft2 As Integer
                                                    Dim TaskbarLeft2 As Integer
                                                    If Orientation2 = True Then
                                                        TrayWndLeft2 = CInt(trayWnd.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                                                        TrayWndWidth2 = CInt(trayWnd.Current.BoundingRectangle.Width.ToString.Replace("-", ""))
                                                        RebarWndLeft2 = CInt(RebarWnd2.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                                                        TaskbarLeft2 = CInt((RebarWndLeft2 - TrayWndLeft2).ToString.Replace("-", ""))
                                                    Else
                                                        TrayWndLeft2 = CInt(trayWnd.Current.BoundingRectangle.Top.ToString.Replace("-", ""))
                                                        TrayWndWidth2 = CInt(trayWnd.Current.BoundingRectangle.Height.ToString.Replace("-", ""))
                                                        RebarWndLeft2 = CInt(RebarWnd2.Current.BoundingRectangle.Top.ToString.Replace("-", ""))
                                                        TaskbarLeft2 = CInt((RebarWndLeft2 - TrayWndLeft2).ToString.Replace("-", ""))
                                                    End If
                                                    Dim Position2 As Integer
                                                    If trayWnd.Current.ClassName = "Shell_TrayWnd" Then
                                                        If CenterBetween = True Then
                                                            Dim offset = (TrayNotify.Current.BoundingRectangle.Width / 2 - (TaskbarLeft2 \ 2))
                                                            Position2 = CInt((TrayWndWidth2 / 2 - (TaskbarWidth2 / 2) - TaskbarLeft2 - offset).ToString.Replace("-", "")) + OffsetPosition
                                                        Else
                                                            Position2 = CInt((TrayWndWidth2 / 2 - (TaskbarWidth2 / 2) - TaskbarLeft2).ToString.Replace("-", "")) + OffsetPosition
                                                        End If
                                                    Else
                                                        Position2 = CInt((TrayWndWidth2 / 2 - (TaskbarWidth2 / 2) - TaskbarLeft2).ToString.Replace("-", "")) + OffsetPosition2
                                                    End If
                                                    XLocationEffect2.SecondTaskbarPtr = CType(TaskList.Current.NativeWindowHandle, IntPtr)
                                                    XLocationEffect2.SecondTaskbarPosition = CInt(Position2.ToString.Replace("-", ""))
                                                    XLocationEffect2.SecondTaskbarOldPosition = CInt(OldPosition2.ToString.Replace("-", ""))
                                                    Console.WriteLine("SecondTaskbarCalculation | OldLeft = " & OldLeft1 & " Left = " & TaskbarLeft2 + TaskbarWidth2 & " <-- If not the same we call the Animator")
                                                    If Not OldLeft2 = TaskbarLeft2 + TaskbarWidth2 Or UpdateTaskbar = True Or TaskbarChanged = True Then
                                                        Console.WriteLine("Call Animator 2")
                                                        Dim t2 As System.Threading.Thread = New System.Threading.Thread(AddressOf AnimationControl2.animateTaskbar2)
                                                        t2.Start()
                                                    End If
                                                    OldPosition2 = Position2
                                                    OldLeft2 = TaskbarLeft2 + TaskbarWidth2
                                                End If
                                                If TaskListID = 3 Then
                                                    If PrimaryTaskbarOnly = True Then
                                                        If Not trayWnd.Current.ClassName = "Shell_TrayWnd" Then
                                                            Continue For
                                                        End If
                                                    End If
                                                    Dim TreeWalker3 As TreeWalker = TreeWalker.ControlViewWalker
                                                    Dim BChildFirst3 As AutomationElement = TreeWalker3.GetFirstChild(TaskList)
                                                    Dim BChildLast3 As AutomationElement = TreeWalker3.GetLastChild(TaskList)
                                                    If TaskList.Current.BoundingRectangle.Height >= 200 Then
                                                        Orientation3 = False
                                                    Else
                                                        Orientation3 = True
                                                    End If
                                                    Dim TaskbarWidth3 As Integer
                                                    If Orientation3 = True Then
                                                        TaskbarWidth3 = CInt((BChildFirst3.Current.BoundingRectangle.Left - TaskList.Current.BoundingRectangle.Left) + (BChildLast3.Current.BoundingRectangle.Left - TaskList.Current.BoundingRectangle.Left) + BChildLast3.Current.BoundingRectangle.Width)
                                                    Else
                                                        TaskbarWidth3 = CInt((BChildFirst3.Current.BoundingRectangle.Top - TaskList.Current.BoundingRectangle.Top) + (BChildLast3.Current.BoundingRectangle.Top - TaskList.Current.BoundingRectangle.Top) + BChildLast3.Current.BoundingRectangle.Height)
                                                    End If
                                                    Dim RebarWnd3 As AutomationElement = AutomationElement.FromHandle(GetParent(CType(TaskList.Current.NativeWindowHandle, IntPtr)))
                                                    Dim TrayWndLeft3 As Integer
                                                    Dim TrayWndWidth3 As Integer
                                                    Dim RebarWndLeft3 As Integer
                                                    Dim TaskbarLeft3 As Integer
                                                    If Orientation3 = True Then
                                                        TrayWndLeft3 = CInt(trayWnd.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                                                        TrayWndWidth3 = CInt(trayWnd.Current.BoundingRectangle.Width.ToString.Replace("-", ""))
                                                        RebarWndLeft3 = CInt(RebarWnd3.Current.BoundingRectangle.Left.ToString.Replace("-", ""))
                                                        TaskbarLeft3 = CInt((RebarWndLeft3 - TrayWndLeft3).ToString.Replace("-", ""))
                                                    Else
                                                        TrayWndLeft3 = CInt(trayWnd.Current.BoundingRectangle.Top.ToString.Replace("-", ""))
                                                        TrayWndWidth3 = CInt(trayWnd.Current.BoundingRectangle.Height.ToString.Replace("-", ""))
                                                        RebarWndLeft3 = CInt(RebarWnd3.Current.BoundingRectangle.Top.ToString.Replace("-", ""))
                                                        TaskbarLeft3 = CInt((RebarWndLeft3 - TrayWndLeft3).ToString.Replace("-", ""))
                                                    End If
                                                    Dim Position3 As Integer
                                                    If trayWnd.Current.ClassName = "Shell_TrayWnd" Then
                                                        If CenterBetween = True Then
                                                            Dim offset = (TrayNotify.Current.BoundingRectangle.Width / 2 - (TaskbarLeft3 \ 2))
                                                            Position3 = CInt((TrayWndWidth3 / 2 - (TaskbarWidth3 / 2) - TaskbarLeft3 - offset).ToString.Replace("-", "")) + OffsetPosition
                                                        Else
                                                            Position3 = CInt((TrayWndWidth3 / 2 - (TaskbarWidth3 / 2) - TaskbarLeft3).ToString.Replace("-", "")) + OffsetPosition
                                                        End If
                                                    Else
                                                        Position3 = CInt((TrayWndWidth3 / 2 - (TaskbarWidth3 / 2) - TaskbarLeft3).ToString.Replace("-", "")) + OffsetPosition2
                                                    End If
                                                    XLocationEffect3.ThirdTaskbarPtr = CType(TaskList.Current.NativeWindowHandle, IntPtr)
                                                    XLocationEffect3.ThirdTaskbarPosition = CInt(Position3.ToString.Replace("-", ""))
                                                    XLocationEffect3.ThirdTaskbarOldPosition = CInt(OldPosition3.ToString.Replace("-", ""))
                                                    Console.WriteLine("ThirdTaskbarCalculation | OldLeft = " & OldLeft3 & " Left = " & TaskbarLeft3 + TaskbarWidth3 & " <-- If not the same we call the Animator")
                                                    If Not OldLeft3 = TaskbarLeft3 + TaskbarWidth3 Or UpdateTaskbar = True Or TaskbarChanged = True Then
                                                        Console.WriteLine("Call Animator 3")
                                                        Dim t3 As System.Threading.Thread = New System.Threading.Thread(AddressOf AnimationControl3.animateTaskbar3)
                                                        t3.Start()
                                                    End If
                                                    OldPosition3 = Position3
                                                    OldLeft3 = TaskbarLeft3 + TaskbarWidth3
                                                End If
                                            Next
                                            If UpdateTaskbar = True Then
                                                UpdateTaskbar = False
                                                AnimationControl.TaskbarRefresh = True
                                            End If
                                            TaskbarChanged = False
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                If ex.ToString.Contains("E_ACCESSDENIED") Then
                    Dim Handle As IntPtr
                    Dim Laps2 As Integer
                    ClearMemory()
                    Do
                        Laps2 = Laps2 + 1
                        If Laps2 = 50 Then
                            Laps2 = 0
                            ClearMemory()
                        End If
                        Handle = Nothing
                        System.Threading.Thread.Sleep(250)
                        Handle = FindWindowByClass("Shell_TrayWnd", CType(0, IntPtr))
                    Loop Until Not Handle = Nothing
                    Application.Restart()
                    End
                End If
            End Try
            Laps = Laps + 1
            If Laps = 150 Then
                Laps = 0
                ClearMemory()
            End If
        Loop
    End Sub

    Public Shared Sub RefreshWindowsExplorer()
        SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, True, 0)
        Dim CLSID_ShellApplication As Guid = New Guid("13709620-C279-11CE-A49E-444553540000")
        Dim shellApplicationType As Type = Type.GetTypeFromCLSID(CLSID_ShellApplication, True)
        Dim shellApplication As Object = Activator.CreateInstance(shellApplicationType)
        Dim windows As Object = shellApplicationType.InvokeMember("Windows", System.Reflection.BindingFlags.InvokeMethod, Nothing, shellApplication, New Object(-1) {})
        Dim windowsType As Type = windows.GetType
        Dim count As Object = windowsType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, Nothing, windows, Nothing)
        Dim i As Integer = 0
        Do While (i < CType(count, Integer))
            Dim item As Object = windowsType.InvokeMember("Item", System.Reflection.BindingFlags.InvokeMethod, Nothing, windows, New Object() {i})
            Dim itemType As Type = item.GetType
            Dim itemName As String = CType(itemType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, Nothing, item, Nothing), String)
            If (itemName = "Shell_TrayWnd") Then
                itemType.InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod, Nothing, item, Nothing)
            End If
            i = (i + 1)
        Loop
    End Sub

    Public Shared Function ClearMemory() As Int32
        Return SetProcessWorkingSetSize(Diagnostics.Process.GetCurrentProcess.Handle, 2097152, 2097152)
    End Function

    Private Shared Function ToAbgr(ByVal color As Color) As UInteger
        Return ((CType(color.A, UInteger) + 24) _
                    Or ((CType(color.B, UInteger) + 16) _
                    Or ((CType(color.G, UInteger) + 8) _
                    Or color.R)))
    End Function

    Public Shared Sub EnableTaskbarStyle()
        Dim Progman As AutomationElement = AutomationElement.FromHandle(FindWindowByClass("Progman", CType(0, IntPtr)))
        Dim desktops As AutomationElement = AutomationElement.RootElement
        Dim condition As New OrCondition(New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_TrayWnd"), New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_SecondaryTrayWnd"))
        Dim lists As AutomationElementCollection = desktops.FindAll(TreeScope.Children, condition)
        Dim accent = New AccentPolicy()
        Dim accentStructSize = Marshal.SizeOf(accent)
        If TaskbarStyle = 1 Then
            accent.AccentState = AccentState.ACCENT_ENABLE_TRANSPARANT

        End If
        If TaskbarStyle = 3 Then

            accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND
            accent.GradientColor = ToAbgr(blurColor) ''&H10000000 'AARRGGBB
        End If
        If TaskbarStyle = 2 Then
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND

        End If
        Dim accentPtr = Marshal.AllocHGlobal(accentStructSize)
        Marshal.StructureToPtr(accent, accentPtr, False)
        Dim data = New WindowCompositionAttributeData With {
            .Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
            .SizeOfData = accentStructSize,
            .Data = accentPtr
        }
        Dim trays As New ArrayList
        For Each trayWnd As AutomationElement In lists
            trays.Add(trayWnd.Current.NativeWindowHandle.ToString)
        Next
        Do
            For Each tray As String In trays
                Dim trayptr As IntPtr = CType(tray.ToString, IntPtr)
                SetWindowCompositionAttribute(CType(trayptr, IntPtr), data)
            Next
            System.Threading.Thread.Sleep(14)
        Loop Until TaskbarTransparant = False Or UpdateTaskbarStyle = True
        RefreshWindowsExplorer()
        For Each tray As String In trays
            Dim trayptr As IntPtr = CType(tray.ToString, IntPtr)
            SendMessage(trayptr, WM_THEMECHANGED, True, 0)
            SendMessage(trayptr, WM_DWMCOLORIZATIONCOLORCHANGED, True, 0)
            SendMessage(trayptr, WM_DWMCOMPOSITIONCHANGED, True, 0)
        Next
        Marshal.FreeHGlobal(accentPtr)
        If UpdateTaskbarStyle = True Then
            If TaskbarTransparant = True Then
                UpdateTaskbarStyle = False
                Dim t1 As System.Threading.Thread = New System.Threading.Thread(AddressOf Taskbar.EnableTaskbarStyle)
                t1.Start()
            End If
        End If
    End Sub

    Public Shared Sub ResetTaskbarStyle()
        Dim Progman As AutomationElement = AutomationElement.FromHandle(FindWindowByClass("Progman", CType(0, IntPtr)))
        Dim desktops As AutomationElement = AutomationElement.RootElement
        Dim condition As New OrCondition(New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_TrayWnd"), New PropertyCondition(AutomationElement.ClassNameProperty, "Shell_SecondaryTrayWnd"))
        Dim lists As AutomationElementCollection = desktops.FindAll(TreeScope.Children, condition)
        Dim trays As New ArrayList
        For Each trayWnd As AutomationElement In lists
            trays.Add(trayWnd.Current.NativeWindowHandle.ToString)
        Next
        RefreshWindowsExplorer()
        For Each tray As String In trays
            Dim trayptr As IntPtr = CType(tray.ToString, IntPtr)
            SendMessage(trayptr, WM_THEMECHANGED, True, 0)
            SendMessage(trayptr, WM_DWMCOLORIZATIONCOLORCHANGED, True, 0)
            SendMessage(trayptr, WM_DWMCOMPOSITIONCHANGED, True, 0)
        Next
    End Sub

    Public Shared Sub Closing()
        AppClosing = True
        SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, True, 0)
        TaskbarTransparant = False
        System.Threading.Thread.Sleep(50) : Application.DoEvents()
        SendMessage(XLocationEffect.FirstTaskbarPtr, WM_THEMECHANGED, True, 0)
        SendMessage(XLocationEffect.FirstTaskbarPtr, WM_DWMCOLORIZATIONCOLORCHANGED, True, 0)
        SendMessage(XLocationEffect.FirstTaskbarPtr, WM_DWMCOMPOSITIONCHANGED, True, 0)
        SendMessage(XLocationEffect2.SecondTaskbarPtr, WM_THEMECHANGED, True, 0)
        SendMessage(XLocationEffect2.SecondTaskbarPtr, WM_DWMCOLORIZATIONCOLORCHANGED, True, 0)
        SendMessage(XLocationEffect2.SecondTaskbarPtr, WM_DWMCOMPOSITIONCHANGED, True, 0)
        SendMessage(XLocationEffect3.ThirdTaskbarPtr, WM_THEMECHANGED, True, 0)
        SendMessage(XLocationEffect3.ThirdTaskbarPtr, WM_DWMCOLORIZATIONCOLORCHANGED, True, 0)
        SendMessage(XLocationEffect3.ThirdTaskbarPtr, WM_DWMCOMPOSITIONCHANGED, True, 0)
        System.Threading.Thread.Sleep(500) : Application.DoEvents()
        SendMessage(GetParent(Shell_TrayWndPtr), WM_SETREDRAW, True, 0)
        SendMessage(ReBarWindow32Ptr, WM_SETREDRAW, True, 0)
        SetWindowPos(XLocationEffect.FirstTaskbarPtr, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
        SetWindowPos(XLocationEffect2.SecondTaskbarPtr, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
        SetWindowPos(XLocationEffect3.ThirdTaskbarPtr, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE Or SWP_ASYNCWINDOWPOS Or SWP_NOACTIVATE Or SWP_NOZORDER Or SWP_NOSENDCHANGING)
        End
    End Sub

    Public Shared Sub OnClose(sender As Object, e As EventArgs)
        Closing()
    End Sub

End Class