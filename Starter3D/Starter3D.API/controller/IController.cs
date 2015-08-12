using Starter3D.API.utils;

namespace Starter3D.API.controller
{
  /// <summary>
  /// A controller is the class in charge of controling the logic of a Starter3D plugin, all plugin must implement this interface
  /// to be recognized as such.
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IController
  {
    int Width { get; }              //Desired width for the application window
    int Height { get; }             //Desired height for the application window
    bool IsFullScreen { get; }      //Sets the applications window to full screen mode, overriding the width and height parameters
    object CentralView { get; }     //GUI UserControl that will be placed on the center of the window, as an overlay of the rendering canvas
    object LeftView { get; }        //GUI UserControl that will be placed to the left of the rendering canvas
    object RightView { get; }       //GUI UserControl that wll be placed to the right of the rendering canvas
    object TopView { get; }         //GUI UserControl that wll be placed on the top of the window
    object BottomView { get; }      //GUI UserControl that wll be placed on the bottom of the window  
    bool HasUserInterface { get; }  //Indicates if this plugin contains a user interface 
    string Name { get; }            //Name of the plugin, to be displayed as the title of the window
        
    /// <summary>
    /// Load method is called at the beginning of the application. All the initial configuration and setup should be performed here
    /// </summary>
    void Load();            

    /// <summary>
    /// Method in charge of rendering to the canvas, any rendering code needs to be here. The method will be called according to the frame rate defined for the application
    /// </summary>
    /// <param name="deltaTime">Time elapsed between this call and the previous rendering call</param>
    void Render(double deltaTime);
    
    /// <summary>
    /// Method called continuosly in the application loop, any logic updates (i.e. not rendering) should be here
    /// </summary>
    /// <param name="deltaTime">Time elapses between this call and the previous update call</param>
    void Update(double deltaTime);

    /// <summary>
    /// Method called when the mouse is pressed
    /// </summary>
    /// <param name="button">Button pressed for this event</param>
    /// <param name="x">Mouse x position when this event occured</param>
    /// <param name="y">Mouse y position when this event occured</param>
    void MouseDown(ControllerMouseButton button, int x, int y);
    
    /// <summary>
    /// Method called when the mouse is released
    /// </summary>
    /// <param name="button">Button released for this event</param>
    /// <param name="x">Mouse x position when this event occured</param>
    /// <param name="y">Mouse y position when this event occured</param>
    void MouseUp(ControllerMouseButton button, int x, int y);
    
    /// <summary>
    /// Method called when the mouse wheel moves
    /// </summary>
    /// <param name="delta">Change detected in the movement of the wheel (positive or negative depending on the direction)</param>
    /// <param name="x">Mouse x position when this event occured</param>
    /// <param name="y">Mouse y position when this event occured</param>
    void MouseWheel(int delta, int x, int y);
    
    /// <summary>
    /// Method called when the mouse moves
    /// </summary>
    /// <param name="x">Mouse x position when this event occured</param>
    /// <param name="y">Mouse y position when this event occured</param>
    /// <param name="deltaX">Delta between current and previos x position</param>
    /// <param name="deltaY">Delta between current and previos y position</param>
    void MouseMove(int x, int y, int deltaX, int deltaY);

    /// <summary>
    /// Method called when a key in the keyboard is pressed
    /// </summary>
    /// <param name="key">ASCII character for the key (low case)</param>
    void KeyDown(int key);

    /// <summary>
    /// Method called when the size of the canvas changes
    /// </summary>
    /// <param name="width">New width of the canvas</param>
    /// <param name="height">New height of the canvas</param>
    void UpdateSize(double width, double height);
  }
}
