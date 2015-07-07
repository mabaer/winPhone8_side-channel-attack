# winPhone8_side-channel-attack
In this project we have two Windows Phone 8 apps which perform a collusion attack via a covert channel (MediaLibrary).

This example was created for the course "Mobile Security Seminar" at TU Darmstadt in 2013 by Marc-André Bär.

-App1 ("FakePasswordStorage") is an application which saves passwords and has just capabilities for the MediaLibrary to add pictures to entries but not for the network.
Without the knowledge of the user the application saves the password information in the MediaLibrary.

-App2 ("FakePictureViewer") is an application which should be used as normal picture viewer for local and online pictures. 
The app checks at each start if new password information are in the MediaLibrary. If yes it parses the information and sends it to a server.

