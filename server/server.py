from flask import Flask, jsonify, send_from_directory

app = Flask(__name__)

@app.route("/signature_key", methods=["GET"])
def getSignaturePubKey():
	with open('pbKey.txt') as f:
		return f.read().strip()

@app.route("/licenses/<filename>", methods=["GET"])
def getLicense(filename):
	return send_from_directory(".",filename, as_attachment=True)

if __name__ == '__main__':
	app.run(port=8080, host='0.0.0.0')
