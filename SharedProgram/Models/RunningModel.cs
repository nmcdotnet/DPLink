namespace SharedProgram.Models
{
    public class RunningModel
    {
        public int NumberOfStation { get; set; } = -1;

        public List<RunningStationModel> StationList { get; set; } = new List<RunningStationModel>();

        public RunningModel(int numberOfStation)
        {
            NumberOfStation = numberOfStation;
            ResetDefault();
        }
        public void ResetDefault()
        {
            for (int i = 0; i < NumberOfStation; i++)
            {
                RunningStationModel runningStationModel = new();
                runningStationModel.Index = i;
                StationList.Add(runningStationModel);
            }
        }
    }
}
