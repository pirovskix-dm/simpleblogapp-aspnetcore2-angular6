interface Date {
    toDDMMYYYY(separator: string): string;
}

interface String {
    toDDMMYYYY(separator: string): string;
}

Date.prototype.toDDMMYYYY = function(separator: string): string {

	let d: Date = this;

	let day: string = d.getDate().toString();
	let month: string = (d.getMonth() + 1).toString();
	let year: string = d.getFullYear().toString();
	day = day.length == 1 ? "0" + day : day;
	month = month.length == 1 ? "0" + month : month;
	return [ day, month, year ].join(separator);	
}

String.prototype.toDDMMYYYY = function(separator: string): string {
	let date: number = Date.parse(this.toString());
	if(!date)
		return '';
	return new Date(date).toDDMMYYYY(separator);
}