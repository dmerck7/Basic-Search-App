import DS from 'ember-data';

export default DS.Model.extend({
  patientId: DS.attr('number'),
  patientName: DS.attr('string'),
  //documentId: DS.attr('number'),
  documentName: DS.attr('string'),
  documentDate: DS.attr('date'),
  documentDateDisplay: DS.attr('string'),
  documentRelaventCount: DS.attr('number'),
  content: DS.attr('date')
});