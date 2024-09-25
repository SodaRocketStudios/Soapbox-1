using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SRS.DataPersistence
{
	public static class Encryptor
	{
		private static byte[] key = new byte[16];
		private static byte[] iv = new byte[16];

		public static string Encrypt(string data)
		{
			GenerateIV();
			GenerateKey();

			SymmetricAlgorithm algorithm = Aes.Create();
			ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);

			byte[] inputBuffer = Encoding.Unicode.GetBytes(data);
			byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

			string ivString = Encoding.Unicode.GetString(iv);
			string encryptedString = Convert.ToBase64String(outputBuffer);

			return ivString + encryptedString;
		}

		public static string Decrypt(string encryptedData)
		{
			GenerateIV();
			GenerateKey();

			int endOfIV = iv.Length / 2;

			string ivString = encryptedData.Substring(0, endOfIV);
			byte[] extractedIV = Encoding.Unicode.GetBytes(ivString);

			string encryptedString = encryptedData.Substring(endOfIV);

			SymmetricAlgorithm algorithm = Aes.Create();
			ICryptoTransform transform = algorithm.CreateEncryptor(key, extractedIV);

			byte[] inputBuffer = Convert.FromBase64String(encryptedString);
			byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);		

			return Encoding.Unicode.GetString(outputBuffer);
		}

		private static void GenerateKey()
		{
			int sum = 0;
			foreach(char character in Application.productName)
			{
				sum += character;
			}

			System.Random rnd = new System.Random(sum);
			rnd.NextBytes(key);
		}

		private static void GenerateIV()
		{
			System.Random rnd = new System.Random();
			rnd.NextBytes(iv);
		}
	}
}