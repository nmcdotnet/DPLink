﻿using DipesLink.Extensions;
using DipesLink.Views.Extension;
using IPCSharedMemory;
using SharedProgram.Shared;
using System;
using System.Windows;
using static SharedProgram.DataTypes.CommonDataType;

namespace DipesLink.ViewModels
{
    public partial class MainViewModel
    {

        private void PauseButtonCommandEventHandler(object? sender, EventArgs e)
        {
            int index = sender != null ? (int)sender : -1;
            ActionButtonProcess(index, ActionButtonType.Pause);
        }

        private void StopButtonCommandEventHandler(object? sender, EventArgs e) 
        {
            int index = sender != null ? (int)sender : -1;
            var cusMessForStop = CusMsgBox.Show($"Do you want to stop station {index} ?", "", Views.Enums.ViewEnums.ButtonStyleMessageBox.OKCancel, Views.Enums.ViewEnums.ImageStyleMessageBox.Warning);
            if (cusMessForStop)
                ActionButtonProcess(index, ActionButtonType.Stop);
        }
       

        private void StartButtonCommandEventHandler(object? sender, EventArgs e) 
        {
         
            int index = sender != null ? (int)sender : -1;
            if (CheckJobExisting(index,out _))
                ActionButtonProcess(index, ActionButtonType.Start);
            else
            {
                CusAlert.Show($"Station {index+1}: Job not found", Views.Enums.ViewEnums.ImageStyleMessageBox.Warning, true);
            }
        }

        private void TriggerButtonCommandEventHandler(object? sender, EventArgs e)
        {
            int index = sender != null ? (int)sender : -1;
            ActionButtonProcess(index, ActionButtonType.Trigger);
        }

        internal  void ActionButtonProcess(int stationIndex, ActionButtonType buttonType)
        {
            try
            {
                byte[] indexBytes = SharedFunctions.StringToFixedLengthByteArray(stationIndex.ToString(), 1);
                byte[] actionTypeBytes = DataConverter.ToByteArray(buttonType); //SharedFunctions.StringToFixedLengthByteArray(((int)buttonType).ToString(), 1);
                byte[] combineBytes = SharedFunctions.CombineArrays(indexBytes, actionTypeBytes);
                MemoryTransfer.SendActionButtonToDevice(listIPCUIToDevice1MB[stationIndex],stationIndex, combineBytes);
            }
            catch (Exception) { }
        }

    }
}
