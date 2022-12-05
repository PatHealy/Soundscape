# Soundscape

Developed in Unity 2021.3.2f1. This does not include the server code for the Soundscape note system -- I'll release that code soon


## How to test it

Open up the `Soundscape` scene. This is the example scene. If you just run the scene, it will launch in `host` mode -- running a server and a client at the same time on localhost. If you run a second client on the same machine, it should connect and you'll have two players playing together! 

The game can run in server, client, or host mode. If you look on the `NetworkManager` gameobject and down to the `NetworkInstancer` component, you'll notice a drop-down that makes toggling behind these super easy. 

The other main thing that should draw your attention is the `Connection Data` contained in the `UnityTransport` component on this same gameobject. Here you will find the address, port, and server listen address, which you may need to configure if you want to host the server somewhere others can actually access.

If you know a little about networking, this should be just about all you need to get this thing deployed. If you're new to this, here's a basic guide to get this launched somewhere... 

## How to deploy it

So, the easiest (but not the cheapest, at like $7 per month) way of getting this going is to get a dedicated server running on somewhere like DigitalOcean or AWS. I'd recommend DigitalOcean, if only because that's the one I've actually tried doing this with. 

Here's what you'll need to do:

- Launch a DigitalOcean 'droplet' running Ubuntu 22 (you can keep it to whatever is the cheapest tier of machine)
- (Optionally) set firewall rules to allow inbound TCP and UDP traffic on port 7777
- Take the public IP of the 'droplet' and put it as the 'Address' in the `UnityTransport` connection data. Set the port to `7777` and Server Listen Address to `0.0.0.0`
- Make two builds of the game: (1) set the `NetworkInstancer` state to 'Server' and make a Dedicated Server on Linux build, (2) set the `NetworkInstancer` state to 'Client' and make a standalone build for Windows (or whatever platform you want the client to connect from)
- Use scp to upload your server build to your DigitalOcean droplet
- SSH into your droplet (DigitalOcean makes this super easy with their in-browser console) and run your server executable
- If you run the client, you should be able to connect! 


