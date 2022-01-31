package javaCrypto;

import java.io.UnsupportedEncodingException;
import java.security.InvalidKeyException;
import java.security.Key;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.KeyGenerator;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;

public class Q1CipherMethod {
	
	private static String plainText = "JCA is a powerful tool";
	private static String ciperText = "[B@5474c6c";

	public static void main(String[] args) throws NoSuchAlgorithmException, UnsupportedEncodingException {
		try {
//			encryptCrypto(plainText);
			decryptCrypto(ciperText);
		} catch (InvalidKeyException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (NoSuchPaddingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalBlockSizeException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (BadPaddingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}
	
	public static SecretKey keyGen() throws NoSuchAlgorithmException {
		KeyGenerator keyGenerator = KeyGenerator.getInstance("DES");
		SecureRandom secureRandom = new SecureRandom();
		int keyBitSize = 56; // The size of the key in DES-128
		keyGenerator.init(keyBitSize, secureRandom);
		SecretKey secretKey = keyGenerator.generateKey();
		System.out.println("Key: " + secretKey);
		
		return secretKey;
	}
	
	public static void encryptCrypto (String text) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException {
		SecretKey secretKey= keyGen();
		
		Cipher cipher = Cipher.getInstance("DES");
		cipher.init(Cipher.ENCRYPT_MODE, secretKey);
//		cipher.init(Cipher.DECRYPT_MODE, secretKey);
		byte[] cipherText = cipher.doFinal(text.getBytes());
		
		System.out.println("CipherText: " + cipherText);
	}
	
	public static void decryptCrypto (String text) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException {
		SecretKey secretKey= keyGen();
		
		Cipher cipher = Cipher.getInstance("DES");
//		cipher.init(Cipher.ENCRYPT_MODE, secretKey);
		cipher.init(Cipher.DECRYPT_MODE, secretKey);
		byte[] plainText = cipher.doFinal(text.getBytes());
		
		System.out.println("PlainText: " + plainText);
	}
	

}
