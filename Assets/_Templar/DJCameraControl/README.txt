Script made by TWKTemplar#5881

You put the names of any players you want white listed into the "_DJ Camera System_ (WhiteList)" White Listed names feild. 
"[1] Local Player" is the name of the default unity client for when testing in unity.
To add a name to the "_DJ Camera System_ (WhiteList)" you increase the Size int by 1 under the words 'White listed names'
Then there will be a new blank text feild, caps and special characters are important.

The DJCamera only shows the default layer, the player, and the Mirror reflection layers. This along with the Camera's meshes being on the UI
layer means that the cameras are invisable to each other.

The object named 'CameraOverrideSphere' is a shader on a pickup with a screen space shader set to the DJCamera's render texture.
While in play mode the 'CameraOverrideSphere' is hidden untill a camera is selected as the target through the "CameraUiButton (Move&ScaleMe!)" button.

The object named "CameraTextToggle" is a button that toggles the text for any cameras in the scene as TextMeshPro only renders on the default layer and
so the other cameras can sometimes see the text of other cameras. 

How to make more cameras: 
Duplicate the object named "Camera Parent & Settings (Dupe Me! & Edit me!)" 
DO NOT take it out from the DJCameraManager as the DJCameraManager requires that "Camera Parent & Settings (Dupe Me! & Edit me!)"
and duplidates of it are child objects. 

Move and rotate "FantomCamera (Move&ScaleMe!)" to anywhere in the scene

Move and rotate "CameraUiButton (Move&ScaleMe!)" to anywhere in the scene

To change color, and camera name go into the "Camera Parent & Settings (Dupe Me! & Edit me!)" settings where you can edit
the label, Material Color, and if it tracks the local player around. 

Thats it :)