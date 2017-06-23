using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine; // To access camera class :P
using RakNet;
namespace WizardsTower
{
    class otherClients
    {
        public int id;
        public int xPos;
        public int yPos;
        public int clientAddress;
        public otherClients(int id, int xPos, int yPos, int address)
        {
            this.id = id;
            this.xPos = xPos;
            this.yPos = yPos;
            this.clientAddress = address;
        }
    }
    class Client : GameObject
    {
        public Client(ContentManager content)
        {
            otherTexture = content.Load<Texture2D>(@"Textures\Sprites\Player\Afk");
            frameWidth = 64;
            frameHeight = 64;
        }
        static ushort port = 0;
        public enum GameMessages
        {
            SPAWN_POSITION = DefaultMessageIDTypes.ID_USER_PACKET_ENUM+100,
            POSITION_UPDATE = DefaultMessageIDTypes.ID_USER_PACKET_ENUM+101,
            YOUR_TURN = DefaultMessageIDTypes.ID_USER_PACKET_ENUM+102,
            WELCOME = DefaultMessageIDTypes.ID_USER_PACKET_ENUM+103,
            NEW_CLIENT = DefaultMessageIDTypes.ID_USER_PACKET_ENUM+104,
            REMOVE_DISCONNECTED_CLIENT = DefaultMessageIDTypes.ID_USER_PACKET_ENUM+105
        }
        List<otherClients> otherClientsList = new List<otherClients>();
        Texture2D otherTexture;
        Player playerMethods;
        int other_client_id, other_client_x = 99, other_client_y = 99, other_address;
        int test = 1337;
        string strPort = "0";
        RakPeerInterface peer = RakPeerInterface.GetInstance();
        public Packet packet = new Packet();
        bool connected = false;
        RakString rs;
        int int_message, client_id = 0; // Or VS gives us an error that its not initialized in the if statement after message loop :S
        public int posX = 0, posY = 0; // We need to initialize it or visual gives error later :S
        int x ,y;
        BitStream bsOut = new BitStream();
        SystemAddress server_address = new SystemAddress();
        int removeID;
        int removeAddress;
        public void initialize()
        {
            peer.Startup(1, new SocketDescriptor(port, ""), 1);
            Console.WriteLine("Starting the client.");
        // 90.229.195.46
            peer.Connect("172.18.5.57", 60000,"0",0);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = SpriteEffects.None;
            foreach(otherClients oClient in otherClientsList)
            {
                spriteBatch.Draw(otherTexture, Camera.WorldToScreen(new Rectangle(oClient.xPos * frameWidth, oClient.yPos * frameHeight, 64,64)), new Rectangle(0,0,64,64), Color.White, 0.0f, Vector2.Zero, effect, drawDepth);
            }
        }
        public void Update(GameTime gameTime, ref Player player)
        {
                for(packet = peer.Receive(); packet != null; peer.DeallocatePacket(packet), packet = peer.Receive())
                {
                    BitStream bsIn = new BitStream(packet.data, packet.length, false);
                    bsIn.IgnoreBytes(1);
                    switch(packet.data[0])
                    {
                    case (byte)GameMessages.REMOVE_DISCONNECTED_CLIENT:
                 /*   bsIn.Read(out removeID); // Receive the disconnected clients ID
                    bsIn.Read(out removeAddress);
                    foreach(otherClients dcClient in otherClientsList)
                    {
                        if(dcClient.clientAddress == removeAddress)
                        {
                            otherClientsList.Remove(dcClient);
                            break;
                        }
                    }*/
                    break;
                        case (byte)GameMessages.SPAWN_POSITION:
                        bsIn.Read(out client_id);
                        bsIn.Read(out posX); // Receive spawn location
                        bsIn.Read(out posY); // Receive spawn
                        Console.WriteLine(posY);
                        // Repos of camera might be needed here aswell.
                        Console.WriteLine("We are Client Number: {0} at location: {1},{2}", int_message,posX,posY);
                        player.xPos = posX;
                        player.yPos = posY;
                        server_address = packet.systemAddress;
                        break;
                        case (byte)GameMessages.WELCOME:
                        Console.WriteLine("Server said we are client number: " + int_message);
                        break;
                        case (byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                        connected = true;
                        Console.WriteLine("Connection Accepted");
                        break;
                        case (byte)GameMessages.POSITION_UPDATE: // Receive positions
                        bsIn.Read(out other_client_id);
                        bsIn.Read(out other_client_x);
                        bsIn.Read(out other_client_y);
                        foreach(otherClients oClient in otherClientsList)
                        {
                            if(oClient.id == other_client_id && oClient.id != client_id)
                            {
                                oClient.xPos = other_client_x;
                                oClient.yPos = other_client_y;
                            }
                        }
                        Console.WriteLine("Received another clients position");
                            Console.WriteLine("Received another clients position");
                        break;
                        case (byte)GameMessages.YOUR_TURN:
                        /*bsOut.Reset();
                        Console.WriteLine("My turn, sending message.");
                        bsOut.Write((byte)GameMessages.POSITION_UPDATE);  // Send position
                        bsOut.Write(client_id);
                        bsOut.Write(20); // Our X position
                        bsOut.Write(20); // Our Y position
                        Console.WriteLine("Position sent");
                        peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, false);
                       bsOut.Reset(); */
                        break;
                        case (byte)GameMessages.NEW_CLIENT:
                            // Receive given data./
                    /*
                            bsIn.Read(out other_client_id); // Receive the given ID
                            bsIn.Read(out other_client_x);
                            bsIn.Read(out other_client_y); */
                        // Do stuff with new client
                            bsIn.Read(out other_client_id);
                            bsIn.Read(out other_client_x);
                            bsIn.Read(out other_client_y);
                            otherClientsList.Add(new otherClients(other_client_id, other_client_x, other_client_y, other_address));
                            break;
                        default:
                            Console.WriteLine("Message with identifier " + packet.data[0] + " has arrived.");
                            break;
                    }
                    bsOut.Reset();
            }
                    //Code to send our position to the server.
                    if(true)
                    {
                    Console.WriteLine("We (Client {0}) sent our position: {1},{2}", client_id,player.xPos,player.yPos);
                    bsOut.Reset();
                    bsOut.Write((byte)GameMessages.POSITION_UPDATE);
                    bsOut.Write(client_id);
                    bsOut.Write(player.xPos);
                    bsOut.Write(player.yPos);
                    peer.Send(bsOut, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, server_address, false);
                    }
            Console.WriteLine("We are client {0}.", client_id);
        }
     }
}