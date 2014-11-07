//----------------------------------------------------------------------------
//  This file is part of the OpenNETCF Smart Device Framework Code Samples.
// 
//  Copyright (C) OpenNETCF Consulting, LLC.  All rights reserved.
// 
//  This source code is intended only as a supplement to Smart Device 
//  Framework and/or on-line documentation.  
// 
//  THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

// UPNP Control Point interfaces, classes and constants

namespace UPnPFinder
{
    public class Constants
    {
        public const int DISPID_NEWENUM = -4;
        public const int DISPID_VALUE = 0;
        public const int DISPID_XOBJ_MIN = 0x60020000;
        public const int DISPID_XOBJ_MAX = 0x6002FFFF;
        public const int DISPID_XOBJ_BASE = DISPID_XOBJ_MIN;

        public const int DISPID_UPNPFINDDEVICES = (DISPID_XOBJ_BASE + 1000);
        public const int DISPID_UPNPFINDDEVICESCALLBACK = (DISPID_UPNPFINDDEVICES + 500);
        public const int DISPID_UPNPSERVICES = (DISPID_UPNPFINDDEVICESCALLBACK + 500);
        public const int DISPID_UPNPSERVICE = (DISPID_UPNPSERVICES + 500);
        public const int DISPID_UPNPDEVICES = (DISPID_UPNPSERVICE + 1000);
        public const int DISPID_UPNPDEVICE = (DISPID_UPNPDEVICES + 500);
        public const int DISPID_UPNPDESCRIPTIONDOC = (DISPID_UPNPDEVICE + 500);

        public const int DISPID_UPNPFINDDEVICES_FINDBYTYPE = (DISPID_UPNPFINDDEVICES + 1);
        public const int DISPID_UPNPFINDDEVICES_FINDBYDCPI = (DISPID_UPNPFINDDEVICES + 2);
        public const int DISPID_UPNPFINDDEVICES_FINDBYUDN = (DISPID_UPNPFINDDEVICES + 3);
        public const int DISPID_UPNPFINDDEVICES_CREATEASYNCFIND = (DISPID_UPNPFINDDEVICES + 4);
        public const int DISPID_UPNPFINDDEVICES_STARTASYNCFIND = (DISPID_UPNPFINDDEVICES + 5);
        public const int DISPID_UPNPFINDDEVICES_CANCELASYNCFIND = (DISPID_UPNPFINDDEVICES + 6);

        public const int DISPID_UPNPFINDDEVICESCALLBACK_NEWDEVICE = (DISPID_UPNPFINDDEVICESCALLBACK + 1);
        public const int DISPID_UPNPFINDDEVICESCALLBACK_SEARCHCOMPLETE = (DISPID_UPNPFINDDEVICESCALLBACK + 2);

        public const int DISPID_UPNPSERVICES_COUNT = (DISPID_UPNPSERVICES + 1);

        public const int DISPID_UPNPSERVICE_QUERYSTATEVARIABLE = (DISPID_UPNPSERVICE + 1);
        public const int DISPID_UPNPSERVICE_INVOKEACTION = (DISPID_UPNPSERVICE + 2);
        public const int DISPID_UPNPSERVICE_SERVICETYPEIDENTIFIER = (DISPID_UPNPSERVICE + 3);
        public const int DISPID_UPNPSERVICE_ADDSTATECHANGECALLBACK = (DISPID_UPNPSERVICE + 4);
        public const int DISPID_UPNPSERVICE_SERVICEID = (DISPID_UPNPSERVICE + 5);
        public const int DISPID_UPNPSERVICE_LASTTRANSPORTSTATUS = (DISPID_UPNPSERVICE + 6);

        public const int DISPID_UPNPDEVICES_COUNT = (DISPID_UPNPDEVICES + 1);

        public const int DISPID_UPNPDEVICE_ISROOTDEVICE = (DISPID_UPNPDEVICE + 1);
        public const int DISPID_UPNPDEVICE_ROOT = (DISPID_UPNPDEVICE + 2);
        public const int DISPID_UPNPDEVICE_PARENT = (DISPID_UPNPDEVICE + 3);
        public const int DISPID_UPNPDEVICE_HASCHILDREN = (DISPID_UPNPDEVICE + 4);
        public const int DISPID_UPNPDEVICE_CHILDREN = (DISPID_UPNPDEVICE + 5);
        public const int DISPID_UPNPDEVICE_UDN = (DISPID_UPNPDEVICE + 6);
        public const int DISPID_UPNPDEVICE_FRIENDLYNAME = (DISPID_UPNPDEVICE + 7);
        public const int DISPID_UPNPDEVICE_DEVICETYPE = (DISPID_UPNPDEVICE + 8);
        public const int DISPID_UPNPDEVICE_PRESENTATIONURL = (DISPID_UPNPDEVICE + 9);
        public const int DISPID_UPNPDEVICE_MANUFACTURERNAME = (DISPID_UPNPDEVICE + 10);
        public const int DISPID_UPNPDEVICE_MANUFACTURERURL = (DISPID_UPNPDEVICE + 11);
        public const int DISPID_UPNPDEVICE_MODELNAME = (DISPID_UPNPDEVICE + 12);
        public const int DISPID_UPNPDEVICE_MODELNUMBER = (DISPID_UPNPDEVICE + 13);
        public const int DISPID_UPNPDEVICE_DESCRIPTION = (DISPID_UPNPDEVICE + 14);
        public const int DISPID_UPNPDEVICE_MODELURL = (DISPID_UPNPDEVICE + 15);
        public const int DISPID_UPNPDEVICE_UPC = (DISPID_UPNPDEVICE + 16);
        public const int DISPID_UPNPDEVICE_SERIALNUMBER = (DISPID_UPNPDEVICE + 17);
        public const int DISPID_UPNPDEVICE_LOADSMALLICON = (DISPID_UPNPDEVICE + 18);
        public const int DISPID_UPNPDEVICE_LOADICON = (DISPID_UPNPDEVICE + 19);
        public const int DISPID_UPNPDEVICE_SERVICES = (DISPID_UPNPDEVICE + 20);

        public const int DISPID_UPNPDESCRIPTIONDOC_LOAD = (DISPID_UPNPDESCRIPTIONDOC + 1);
        public const int DISPID_UPNPDESCRIPTIONDOC_LOADASYNC = (DISPID_UPNPDESCRIPTIONDOC + 2);
        public const int DISPID_UPNPDESCRIPTIONDOC_LOADERROR = (DISPID_UPNPDESCRIPTIONDOC + 3);
        public const int DISPID_UPNPDESCRIPTIONDOC_ABORT = (DISPID_UPNPDESCRIPTIONDOC + 4);
        public const int DISPID_UPNPDESCRIPTIONDOC_ROOTDEVICE = (DISPID_UPNPDESCRIPTIONDOC + 5);
        public const int DISPID_UPNPDESCRIPTIONDOC_DEVICEBYUDN = (DISPID_UPNPDESCRIPTIONDOC + 6);

    }

    [
        Guid("ADDA3D55-6F72-4319-BFF9-18600A539B10"),
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUPnPDeviceFinder
    {
        [DispId(Constants.DISPID_UPNPFINDDEVICES_FINDBYTYPE)]
        IUPnPDevices FindByType(string bstrTypeURI, uint dwFlags);

        [DispId(Constants.DISPID_UPNPFINDDEVICES_CREATEASYNCFIND)]
        int CreateAsyncFind(string bstrTypeURI, int dwFlags, IntPtr punkDeviceFinderCallback);

        [DispId(Constants.DISPID_UPNPFINDDEVICES_STARTASYNCFIND)]
        void StartAsyncFind(int lFindData);

        [DispId(Constants.DISPID_UPNPFINDDEVICES_CANCELASYNCFIND)]
        void CancelAsyncFind(int lFindData);

        [DispId(Constants.DISPID_UPNPFINDDEVICES_FINDBYUDN)]
        IUPnPDevice FindByUDN(string bstrUDN);
    }

    [
        Guid("415A984A-88B3-49F3-92AF-0508BEDF0D6C"),
        ComVisible(true),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUPnPDeviceFinderCallback
    {
        [PreserveSig]
        IntPtr DeviceAdded(int lFindData,
                            IUPnPDevice pDevice);

        [PreserveSig]
        IntPtr DeviceRemoved(int lFindData,
                              string bstrUDN);

        [PreserveSig]
        IntPtr SearchComplete(int lFindData);
    }

    [
        Guid("FDBC0C73-BDA3-4C66-AC4F-F2D96FDAD68C"),
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUPnPDevices
    {
        [DispId(Constants.DISPID_UPNPDEVICES_COUNT)]
        int Count { get; }

        [DispId(Constants.DISPID_NEWENUM)]
        IntPtr _NewEnum { get; }

        [DispId(Constants.DISPID_VALUE)]
        IUPnPDevice Item(string bstrUDN);

    }


    [
        Guid("3D44D0D1-98C9-4889-ACD1-F9D674BF2221"),
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUPnPDevice
    {
        [DispId(Constants.DISPID_UPNPDEVICE_ISROOTDEVICE)]
        bool IsRootDevice { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_ROOT)]
        //helpstring("returns the top device in the device tree")]
        IUPnPDevice RootDevice { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_PARENT)]
        //helpstring("returns the parent of the current device")]
        IUPnPDevice ParentDevice { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_HASCHILDREN)]
        // helpstring("denotes whether the current device contains child devices")]
        bool HasChildren { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_CHILDREN)]
        // helpstring("returns a collection of the children of the current device")]
        IUPnPDevices Children { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_UDN)]
        // helpstring("returns the UDN of the device")]
        string UniqueDeviceName { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_FRIENDLYNAME)]
        // helpstring("returns the (optional) display name of the device")]
        string FriendlyName { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_DEVICETYPE)]
        // helpstring("returns the device type URI")]
        string Type { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_PRESENTATIONURL)]
        // helpstring("obtains a presentation URL to a web page that can control the device")]
        string PresentationURL { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_MANUFACTURERNAME)]
        // helpstring("displayable manufacturer name")]
        string ManufacturerName { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_MANUFACTURERURL)]
        // helpstring("URL to the manufacturer's website")]
        string ManufacturerURL { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_MODELNAME)]
        // helpstring("a displayable string containing the model name")]
        string ModelName { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_MODELNUMBER)]
        // helpstring("a displayable string containing the model number")]
        string ModelNumber { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_DESCRIPTION)]
        // helpstring("displayable summary of the device's function")]
        string Description { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_MODELURL)]
        // helpstring("URL to a webpage containing model-specific information")]
        string ModelURL { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_UPC)]
        // helpstring("displayable product code")]
        string UPC { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_SERIALNUMBER)]
        // helpstring("displayable serial number")]
        string SerialNumber { get; }

        [DispId(Constants.DISPID_UPNPDEVICE_LOADICON)]
        [PreserveSig]
        // helpstring("retrieves an url from which an icon of the specified format can be loaded")]
        int IconURL(string bstrEncodingFormat, int lSizeX, int lSizeY, int lBitDepth, [MarshalAs(UnmanagedType.BStr)] out string url);

        [DispId(Constants.DISPID_UPNPDEVICE_SERVICES)]
        // helpstring("returns the collection of services exposed by the device")]
        IUPnPServices Services { get; }
    }

    [
        Guid("3F8C8E9E-9A7A-4DC8-BC41-FF31FA374956"),
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUPnPServices
    {
        [DispId(Constants.DISPID_UPNPSERVICES_COUNT)]
        int Count { get; }

        [DispId(Constants.DISPID_NEWENUM)]
        IntPtr _NewEnum { get; }

        [DispId(Constants.DISPID_VALUE)]
        IUPnPService Item(string bstrServiceId);
    }


    [
        Guid("A295019C-DC65-47DD-90DC-7FE918A1AB44"),
        ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUPnPService
    {
        [DispId(Constants.DISPID_UPNPSERVICE_QUERYSTATEVARIABLE)]
        // helpstring("method QueryStateVariable")]
        object QueryStateVariable(string bstrVariableName);

        [DispId(Constants.DISPID_UPNPSERVICE_INVOKEACTION)]
        // helpstring("method InvokeAction")]
        [PreserveSig]
        int InvokeAction(string bstrActionName,
            //[MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_VARIANT)]                 
            object vInActionArgs,
            //[MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]                 
            out object pvOutActionArgs,
            out object pvRetVal);

        [DispId(Constants.DISPID_UPNPSERVICE_SERVICETYPEIDENTIFIER)]
        // helpstring("property ServiceTypeIdentifier")]
        string ServiceTypeIdentifier { get; }

        [DispId(Constants.DISPID_UPNPSERVICE_ADDSTATECHANGECALLBACK)]
        // helpstring("method AddStateChangeCallback")]
        void AddCallback(object pUnkCallback);

        [DispId(Constants.DISPID_UPNPSERVICE_SERVICEID)]
        // helpstring("property Id")]
        string Id { get; }

        [DispId(Constants.DISPID_UPNPSERVICE_LASTTRANSPORTSTATUS)]
        // helpstring("property LastTransportStatus")]
        int LastTransportStatus { get; }
    }



}
