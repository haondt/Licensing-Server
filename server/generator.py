import sys, base64, json, uuid, hashlib, datetime
from Crypto.PublicKey import RSA
from Crypto.Signature import PKCS1_v1_5
from Crypto.Hash import SHA256
from Crypto.Random import new as Random

# List commands in stdout
def show_helptext():
	print("Commands:")
	for c in ['gen-keys','gen-license','verify-key']:
		print('\t' + c)

# Generate a license key. Uses stdin to get HWID and date.
def generate_license_key(digits=0):
	# Get componenets from stdin
	hwid = input("Hardware ID: ").strip()
	days = input("Number of days license is valid for: ").strip()

	# Set maxmimum date to 50 years from now
	if days.isdigit():
		days = int(days)
		if days > 50 * 365:
			days = 50*365
			print("Defaulting to maximum period; 50 years")
	else:
		days = 30
		print("Defaulted to 30 days.")

	# md5 hash HWID
	key = hashlib.md5(hwid.encode()).hexdigest()

	# convert HWID to 8-digit key
	if digits != 0:
		key =  str(hex(int(key, 16) % 2 ** (digits*4)))[2:].upper()
		key = key[:4] + "-" + key[4:]

	# convert expiry date to 8-digit key
	now = datetime.datetime.now(datetime.timezone.utc)
	future = now + datetime.timedelta(days=days)
	future = future.timestamp()
	future = min(int(future),int(2**32-1))
	future = str(hex(int(future))).upper()[2:]
	expiry = future[:4] + "-" + future[4:]

	# combine components
	key += "-" + expiry

	return key

# Generate a public and private key
def generate_keys():
	# generate keys
	modlen = 2560
	pvKey = RSA.generate(modlen, Random().read)
	pbKey = pvKey.publickey()

	# export keys as unicode strings
	pbKey = pbKey.exportKey("PEM")
	pvKey = pvKey.exportKey("PEM")
	pbKey = pbKey.decode('utf-8')
	pvKey = pvKey.decode('utf-8')

	return pvKey, pbKey

# Save keys (as strings) to files
def save_keys_to_file(pvKey, pbKey):
	with open('pvKey.txt', 'w') as f:
		f.write(pvKey)
	with open('pbKey.txt', 'w') as f:
		f.write(pbKey)

# Sign data using private key and return signature
def get_signature(data, pvKey):
	# Convert key string to key
	key = RSA.importKey(pvKey)

	# build signer
	signer = PKCS1_v1_5.new(key)

	# encode data
	digest = SHA256.new()
	digest.update(data.encode('utf-8'))

	# sign key
	signature = signer.sign(digest)

	# encode signature in base64 and decode to unicode
	return base64.b64encode(signature).decode('utf-8')

# Save license key and signature in license file
# Also adds license key to valid licenses list
def save_license_file(license_key, signature):
	with open('licenses.txt', 'a') as f:
			f.write(license_key + '\n')

	# Convert key and signature to json -> string -> file
	with open('new_license.txt', 'w') as f:
		f.write(json.dumps({
			'license_key':license_key,
			'signature': signature
		}))

# Load keys from file
def load_keys():
	pvKey = ""
	pbKey = ""
	with open('pbKey.txt') as f:
		pbKey = f.read()
	with open('pvKey.txt') as f:
		pvKey = f.read()
	return pbKey, pvKey

# Load license and signature from file
def load_license_file():
	license_key, signature = "",""
	with open('good_license.txt') as f:
		j = json.loads(f.read())
		license_key = j['license_key']
		signature = j['signature']
	return license_key, signature

# Verify that signature matches key and data
def verify_signature(pbKey, signature, data):
	pbKey = pbKey.encode('utf-8')
	data = data.encode('utf-8')

	key = RSA.importKey(pbKey)
	signer = PKCS1_v1_5.new(key)

	digest = SHA256.new()
	digest.update(data)

	return signer.verify(digest, base64.b64decode(signature))

# Possible commands:
# gen-keys
# gen-license
# verify-key
def main():
	if len(sys.argv) == 2:
		if sys.argv[1] == 'gen-keys':
			pvKey, pbKey = generate_keys()
			save_keys_to_file(pvKey, pbKey)
			return

		elif sys.argv[1] == 'gen-license':
			pbKey, pvKey = "",""
			try:
				pbKey, pvKey = load_keys()
			except Exception:
				print("Unable to load keys. Run \'gen-keys\' to generate new keys")
				return

			lKey = generate_license_key(8)
			signature = get_signature(lKey, pvKey)
			save_license_file(lKey, signature)
			print('New license exported to new_license.txt')
			return

		elif sys.argv[1] == 'verify-key':
			key = input("Enter license key: ")
			with open('licenses.txt') as f:
				for line in f:
					if line.strip() == key:
						print('Key is valid')
						return
				print('Key is not valid')
				return
	# Show helptext if command not recognized
	show_helptext()

if __name__ == '__main__':
	main()
