var toggleGUI : boolean;
var TurnImage : Texture2D;
 
function OnTriggerEnter (other : Collider)
{
        toggleGUI = true;
}
 
function OnTriggerExit (other : Collider)
{
        toggleGUI = false;
}
 
function OnGUI ()
{
        if (toggleGUI == true)
        GUI.DrawTexture(Rect(Screen.width / 2-150, 0,300,300), TurnImage);
}