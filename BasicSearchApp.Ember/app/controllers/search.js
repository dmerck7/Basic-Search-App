import Controller from '@ember/controller';
import { tracked } from '@glimmer/tracking';
import { action } from '@ember/object';
import { sort } from '@ember/object/computed';
import $ from 'jquery';

//export default Ember.Controller.extend({
export default class SearchController extends Controller {
    @tracked loading = false;
    debounce = {};
    @tracked documents;
    @tracked sortedDocuments;
    @tracked query='';
    @tracked content;
    @tracked patients = [];
    @tracked selectedPatient=-1;
    @tracked sortValue = 0;
    
    @action
    init() {
        //console.log('init');
        this.get('patients').pushObject(
            Ember.Object.create({
                id:-1,
                name: 'All'
            })
        );
        super.init();
        this.loadDocuments();
    }

    @action
    onKeypress() {
        this.loading=true;
        this.store.unloadAll('document');
        clearTimeout(this.debounce) 
        this.debounce = setTimeout( () => {
            this.loadDocuments();
        }, 1500);
    }

    @action
    onExpandedChange(id, expanded, e) {
        this.loadContent(id)
    }

    @action
    updatePatientValue(value) {
        this.set('value', value);
        this.selectedPatient=value;
     }

     @action
    updateSortValue(value) {
        this.set('value', value);
        this.sortValue=value;
        
        this.sortedDocuments = this.documents.sortBy(this.handleSort());
     }

    loadDocuments() {
        this.loading=true;
        $('body').css({ cursor: 'progress' });
        this.store.query('document', {
            query: this.query
        }).then((docs) => {
            this.documents = docs; 
            this.loading=false;
            $('body').css({ cursor: 'default' });
            this.buildPatientSelect();
            this.sortedDocuments = this.documents.sortBy(this.handleSort());
        });
    }

    loadContent(id) {
        this.store.queryRecord('content', {
            id: id
        }).then((content) => {
            var doc = this.store.peekRecord('document', id);
            var txt = content.get('txt');
            txt = this.markAllInstances(txt, this.query);
            txt = txt.replace(/(?:\r\n|\r|\n)/g, '<br>');

            doc.set("content", txt);
        });
    }

    markAllInstances(txt, query) {
        var queryTokens = this.splitQuery(query)

        queryTokens.forEach( (queryToken) => {         

            // TODO - greatly enhance performance by using indexing infomation such as lineIndex, wordIndex

            // Case insensitive replace
            var reg = new RegExp('('+queryToken[0]+')', 'gi');
            txt = txt.replace(reg, '<mark>$1</mark>');
        });
        return txt;
    }

    splitQuery(query)
    {
        //console.log(query)
        var re = new RegExp('(?<=\")[^\"]*(?=\")|[^\" ]+', 'g');
        var results = query.matchAll(re);
        
        return Array.from(results);
    }

    buildPatientSelect(){
        this.set('patients', []);
        this.get('patients').pushObject(
            Ember.Object.create({
                id:-1,
                name: 'All'
            })
        );
        this.documents.forEach((document, i, documents) => {
            //console.log(this.patients)
            var option = Ember.Object.create({id: document.patientId, name: document.patientName});
            var found = false;
            this.patients.forEach((patient) => {
                if (patient.id==option.id) found=true;
            });
            if (!found){
                this.get('patients').pushObject(option);
            }
        })
    }

    handleSort() {
        switch(Number(this.sortValue)) {
            case 0:
                return 'documentRelevantCount';
            break;
            case 1:
                return 'documentDate';
            break;
            case 2:
                return 'patientName';
            break;
            case 3:
                return 'documentName';
            break;
        }
    }


}


