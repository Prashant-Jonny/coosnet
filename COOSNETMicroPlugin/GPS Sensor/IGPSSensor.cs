namespace Hks.Itprojects.GPS
{
    public interface IGPSSensor {
        Position readPosition();
        void startDeviceAndSeekSignal();
        void stopDevice();
        event PositionEventHandler NewPosition;
        void stopTimer();
}
}