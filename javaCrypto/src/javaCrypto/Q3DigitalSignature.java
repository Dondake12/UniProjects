package javaCrypto;

import java.security.InvalidKeyException;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.Signature;
import java.security.SignatureException;
import java.util.Scanner;

public class Q3DigitalSignature {

	public static void main(String[] args) throws NoSuchAlgorithmException, InvalidKeyException, SignatureException {
		Scanner input = new Scanner(System.in);
		
		System.out.println("Enter your data");
		String data = input.nextLine();

		
		Signature signature = Signature.getInstance("SHA256withRSA");
		//Initializing the key generator
		KeyPairGenerator keyPairGen = KeyPairGenerator.getInstance("RSA");
		keyPairGen.initialize(2048);
		//Generate the keys
		KeyPair keyPair = keyPairGen.generateKeyPair();
		// Sign
		signature.initSign(keyPair.getPrivate());
		byte[] digitalSignature = signature.sign();
		// Verify
		signature.initVerify(keyPair.getPublic());
		boolean verified = signature.verify(digitalSignature);
		
		System.out.println(digitalSignature);

	}

}
