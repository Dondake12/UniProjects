package javaCrypto;

import java.security.InvalidKeyException;
import java.security.Key;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.KeyGenerator;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;

public class Q1Cipher {
	
	private static String plainText = "JCA is a powerful tool";

	public static void main(String[] args) throws InvalidKeyException, NoSuchAlgorithmException, NoSuchPaddingException, IllegalBlockSizeException, BadPaddingException {
		
		KeyGenerator keyGenerator = KeyGenerator.getInstance("DES");
		SecureRandom secureRandom = new SecureRandom();
//		int keyBitSize = 56; // The size of the key in DES-128
//		keyGenerator.init(keyBitSize, secureRandom);
		Key secretKey = keyGenerator.generateKey();
		System.out.println("Key: " + secretKey);
		
//		SecretKey secretKey = keyGenerator.generateKey();
		
		Cipher cipher = Cipher.getInstance("DES");
		Cipher.getInstance("DES/ECB/PKCS5Padding");
		cipher.init(Cipher.ENCRYPT_MODE, secretKey);
//		cipher.init(Cipher.DECRYPT_MODE, key);
		byte[] cipherText = cipher.doFinal(plainText.getBytes());
		
		System.out.println("CipherText: " + cipherText);
	}
	

}
