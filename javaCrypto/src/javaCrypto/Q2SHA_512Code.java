package javaCrypto;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Scanner;

public class Q2SHA_512Code {

	public static void main(String[] args) throws UnsupportedEncodingException, NoSuchAlgorithmException {
		Scanner input = new Scanner(System.in);
		
		System.out.println("Enter your password");
		String password = input.nextLine();

		MessageDigest messageDigest = MessageDigest.getInstance("SHA-512");
//		byte[] data = "abcdefghijklmnopqrstuvxyz".getBytes("UTF-8");
		
		messageDigest.update(password.getBytes());
		byte[] digest = messageDigest.digest();
		
		System.out.println("Hash Code (digest)" + digest);
		
		StringBuffer hexString = new StringBuffer();
		for (int i = 0; i < digest.length; i++) {
			hexString.append(Integer.toHexString(0xFF & digest[i]));
		}
		System.out.println("Hash Code(Hex): " + hexString.toString());;
	}

}
