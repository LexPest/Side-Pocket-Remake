/* csogg
 * Copyright (C) 2000 ymnk, JCraft,Inc.
 *  
 * Written by: 2000 ymnk<ymnk@jcraft.com>
 * Ported to C# from JOrbis by: Mark Crichton <crichton@gimp.org> 
 *   
 * Thanks go to the JOrbis team, for licencing the code under the
 * LGPL, making my job a lot easier.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Library General Public License
 * as published by the Free Software Foundation; either version 2 of
 * the License, or (at your option) any later version.
   
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Library General Public License for more details.
 * 
 * You should have received a copy of the GNU Library General Public
 * License along with this program; if not, write to the Free Software
 * Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 */

namespace DragonOgg.csogg
{
	/// <summary>
	/// Summary description for Packet.
	/// </summary>
	public class Packet
	{
		public byte[] packet_base;
		public int packet;
		public int bytes;
		public int b_o_s;
		public int e_o_s;

		public long granulepos;

		public long packetno; // sequence number for decode; the framing
		// knows where there's a hole in the data,
		// but we need coupling so that the codec
		// (which is in a seperate abstraction
		// layer) also knows about the gap

		public Packet()
		{
			// No constructor
		}
	}
}
