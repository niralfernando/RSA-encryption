/*
Author: Niral Fernando
Email: niral.fernando@gmail.com
Date: 2014-04-8 
RSA Encryption program written in C#
*/

using System;
using System.IO;
using System.Collections;

class Program
{
	static void Main()
	{      
        ulong p = 50929;
        ulong q = 51031;
        ulong n = 2598957799; //3125033603 > 2598957799 >  2054847098 is true

		ulong phiN = (p-1)*(q-1); //2598855840
		ulong e = 2099477;//1048633;//phiN & e should be coprime, 0<e<phiN, 2^20<e<2^30
        ulong d = 394384253;  
 
		string plainText = "Hello D.E.A.R. I am student 20432351. This is my little secret: 43177 and "//
                            + "33181. Wolverine is a fictional character, a superhero that appears in" 
                            + "comic books published by Marvel Comics. Born James Howlett and commonly" 
                            + "known as Logan, Wolverine is a mutant who possesses animal-keen senses," 
                            + "enhanced physical capabilities, and a healing factor that allows him to" 
                            +"recover from virtually any wound, disease, or toxin at an accelerated rate." 
                            +"The healing factor also slows down his aging process, enabling him to live " //
                            +"beyond a normal human lifespan. His powerful healing factor enabled him to " //
                            +"survive having the near-indestructible metal alloy adamantium bonded to his "// 
                            +"skeleton. I am ironman   ."; //characters (with spaces) is divisible by 4
        
        ArrayList words4 = new ArrayList();
        //ArrayList ascii = new ArrayList();          
        char[] characters = plainText.ToCharArray(); //Array of characters for plain text
        
        char[] tmpArray = new char[4];
        int[] asciiArray = new int[plainText.Length]; //array that stores corresponding ascii #s for 
        //each character

        string s;
        //Convert each block of 4 letters to ascii code
        for(int i = 0; i < plainText.Length; i++)
        {
        	for(int j = 0; j <= 3; j++)
        	{   
        		tmpArray[j] = characters[i];
        		asciiArray[i] = Convert.ToInt32(characters[i]);
        		i++;
        	}
        	i--;
        	s = string.Join("",tmpArray); //temporary string with 4 characters combined
        	words4.Add(s);
        }

        //foreach loop to count the # of groups of 4 letters/ascii
        int count = 0;
        foreach(string ala1 in words4 )
        {
        	count++;
        }
        Console.WriteLine("my count value {0}, plainText length = {1}", count, plainText.Length);
        //mi = x1256^3 + x2256^2 + x3256^1 + x4.
        int[] mArray = new int[count];
        int elementCount = 0;
        int sum = 0;
        for(int i = 0; i < count*4; i=i+4)
        { 
           sum = (int) (asciiArray[i] * Math.Pow(256,3) + asciiArray[i+1] * Math.Pow(256,2)
            + asciiArray[i+2] * 256 + asciiArray[i+3] );
           mArray[elementCount] = sum; 
           //Console.WriteLine(sum);
           //Console.WriteLine(elementCount);
           sum = 0;
           elementCount++; 
        }

        //tool for sanity check 
        /*
        int count0 = 0;
        foreach(int i in mArray)
        {
            Console.WriteLine(i);
            count0++;
        } 
        Console.WriteLine(); */
        Console.WriteLine("count0 = {0} ",count);  
        
        //step (5) : ENCRYPTION USING MY PRIVATE DECRYPTION KEY --> DIGITAL SIGNATURE
        //zi ≡mi^d (modn)

        int count1 = 0;
        ulong[] zArray = new ulong[count];
        for(int i = 0; i < mArray.Length; i++)
        {
            zArray[i] = RepeatedSquaring((ulong)mArray[i],(ulong)d,(ulong)n);
            //count1++;
        }
        //sanity check
        /*
        foreach(ulong i in zArray)
        {
            Console.WriteLine(i);
            count1++;
        }
        Console.WriteLine(count1); 
        */
        /////////////DIGITAL SIGNATURE///////////
        ulong[] encryptionArray = new ulong[count];
        for(int i=0; i < mArray.Length;i++)
        {
            encryptionArray[i] = RepeatedSquaring(zArray[i],52741219,3125033603);
        }
        foreach(ulong i in encryptionArray)
        {
            Console.WriteLine(i);
            
        }
        //Console.WriteLine(count1); 

        //WRITE TO A FILE
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("20432351.txt"))
            {
                file.Write(n + " " + e + " " + plainText.Length + " ");

                for(int i = 0; i < encryptionArray.Length; i++)
                {
                    file.Write(encryptionArray[i] + " ");
                }
                file.Close();
            }

	}

    public static ulong RepeatedSquaring(ulong baseForExponent, ulong exponent, ulong mod)
        {
            ulong result = 1;
            while (true)
            {
                if (exponent % 2 == 1) 
                {
                    result = result * baseForExponent % mod;
                }

                exponent = exponent /  2;

                if (exponent == 0)
                {
                    break;
                }
                baseForExponent = baseForExponent * baseForExponent % mod;
            }
            return result;
        }
}
