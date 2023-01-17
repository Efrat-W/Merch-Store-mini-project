namespace Simulator;

public static class Simulator
{
    private static volatile bool Run;



    public static void Init()
    {
        new Thread(() => {

        }).Start();
    }
    public static void Quit() { }
}