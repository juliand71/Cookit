# Cookit
A recipe sharing web app built with ASP.NET Core, Razor Pages, SQL Server 2019, and Entity Framework

https://letscookitapp.com

# Current Status of Project

I have a web app that allows users to create and view recipes. User's can only create recipes after they have registered. Users can only edit or delete recipes that they posted.

Recipes have the following properties:
-Image
-Caption
-Title
-Description
-List of Required Equipment
-List of Required Ingredients
-List of Required Instructions

Plan to add some more things like ratings and comments in the future.


# Log
### 2/27/21 - Deployment Adventures
Today I learned that I need to learn more about Dev Ops, deployment, and similar topics.

I first attempted to deploy this to some VMs. I was originally trying to do it the right way, and have a separate VM to host the SQL DB, and another VM to host the web server. I tried setting up some environment variables and user secrets to make things a bit more secure. I have never deployed to IIS before this point so I did a lot of learning and figuring out how that goes. Once I actually got the server configured I had no idea how to troubleshoot the actual app crashing on startup. My guess is it was related to the database connections, and the Image File Service.

Somewhere along the way I completely screwed up my visual studio project file, and had to wipe out the project on my local machine and re-clone it from here. Thank god for Git.

After banging my head against the wall on that for a while, I decided I would test out the automated magic of deploying to Azure. This seems like it would be awesome if I didn't need to interact with a local file system for handling images. I am not sure what was up with the permissions involving that, but I didn't want to sit around bothering to find out. I am also pretty sure the migration to an Azure SQL database didn't work too well either.

So then I decided to go back to deploying it to a few virtual machines that act more like "on premise" servers even though they are in a remote virtual datacenter. I eventually figured out that most of my issues were centered around the database connection string, permissions to the SQL database in the deployment environment, and the fact that I hard coded the default directory for image files into the application. I did manage to get all those resolved and I have a working web app now.

### 2/21/21
Got all of the CRUD functions centered around Recipe working now. Next I'm going to add some styling so it's not so damn boring to look at all the time.
### 2/20/21
I originally was building this with the MVC template, I decided to switch to Razor Pages as I felt it offered more flexibility. I got a basic app working where you can view and create a Recipe today.

# Documentation, Design, and all that
Coming Soon

