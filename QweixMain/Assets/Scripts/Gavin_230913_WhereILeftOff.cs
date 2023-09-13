/*
 * Bugfixing session
 * -Fixed the Demolitionist not firing projectiles or animating properly. Was missing animation events
 * -We currently were chasing down errors. Added null checks to the OnTriggerExit2D (and Enter) functions to clear those errors.
 * 
 * -Still trying to figure out why the TargetLeftRange Message is being sent after the object is destroyed. It looks like SendMessage is a very old function that should be avoided
 * -Need to figure out what to replace SendMessage with. C# events, C# event Actions or UnityEvents.
 * (Maybe UnityEvents? Either way, need to do more research)
 * 
 * Also need to figure out 
 * 
 */
