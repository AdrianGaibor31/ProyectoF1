class AES {
    constructor(key) {
        // Almacena la clave proporcionada y la convierte en un array de bytes
        this.key = this.utf8ToBytes(key);

        // Genera una tabla de sustitución (S-Box). Esta no es la S-Box real de AES, es una versión simplificada.
        this.sBox = this.generateSBox();
    }

    // Convierte una cadena de texto UTF-8 en un array de bytes
    utf8ToBytes(str) {
        return new TextEncoder().encode(str);
    }

    // Convierte un array de bytes en una cadena de texto UTF-8
    bytesToUtf8(bytes) {
        return new TextDecoder().decode(bytes);
    }

    // Genera una tabla de sustitución (S-Box). Esta es una versión simplificada y no representa la S-Box real de AES.
    generateSBox() {
        let sBox = new Uint8Array(256);
        for (let i = 0; i < 256; i++) {
            sBox[i] = i;
        }
        return sBox;
    }

    // Método para encriptar un bloque de datos. Esta es una implementación simplificada y no representa el cifrado real de AES.
    encryptBlock(block) {
        let encrypted = new Uint8Array(block.length);
        for (let i = 0; i < block.length; i++) {
            encrypted[i] = block[i] ^ this.sBox[i % 256];
        }
        return encrypted;
    }

    // Método para desencriptar un bloque de datos. Esta es una implementación simplificada y no representa la descifrado real de AES.
    decryptBlock(block) {
        let decrypted = new Uint8Array(block.length);
        for (let i = 0; i < block.length; i++) {
            decrypted[i] = block[i] ^ this.sBox[i % 256];
        }
        return decrypted;
    }

    // Método para encriptar un texto completo. Convierte el texto en bytes, lo cifra bloque por bloque y luego lo convierte en una cadena Base64.
    encrypt(plaintext) {
        let bytes = this.utf8ToBytes(plaintext);
        let encryptedBytes = this.encryptBlock(bytes);
        return btoa(String.fromCharCode(...encryptedBytes));
    }

    // Método para desencriptar un texto cifrado. Convierte la cadena Base64 en bytes, los descifra bloque por bloque y luego los convierte en texto UTF-8.
    decrypt(ciphertext) {
        let bytes = Uint8Array.from(atob(ciphertext), c => c.charCodeAt(0));
        let decryptedBytes = this.decryptBlock(bytes);
        return this.bytesToUtf8(decryptedBytes);
    }
}

