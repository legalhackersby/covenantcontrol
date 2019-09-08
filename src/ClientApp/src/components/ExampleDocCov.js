import React, { Component } from 'react';
import axios from 'axios'
import { Col, Grid, Panel, Row, Form, Button } from 'react-bootstrap';
import './ExampleDocCov.css'
import $ from 'jquery';
import _ from 'lodash';

import { Config } from '../Config';
import { Link } from 'react-router-dom';

const covenantTemplate = _.template(`
                          <div uid="<%=id%>" class="covenant panel panel-default">
                                 <div class="panel-heading cov-head">
                                    <div class="row">
                                     <div class="col-sm-9" style="magrin-top:-10px;">
                                        <span><%=type%></span>
                                     </div>
                                     <div class="col-sm-1" style="z-index=10000">
                                        <button uid="<%=id%>" type="button" class="btn btn-info btn-circle btn-ok head-remove-button-<%=id%>">
                                            <i uid="<%=id%>" class="glyphicon glyphicon-ok"></i>
                                        </button>
                                     </div>
                                     <div class="col-sm-1" style="z-index=10000">
                                        <button uid="<%=id%>" type="button" class="btn btn-info btn-circle btn-remove">
                                            <i uid="<%=id%>" class="glyphicon glyphicon-remove"></i>
                                        </button>
                                     </div>                                     
                                 </div>
                                 </div>
                                 <div class="panel-collapse collapse">
                                     <div class="panel-body">
                                         <div class="row">
                                            <div class="task-description col-sm-12"><%=description%></div>
                                         </div>                                    
                                     <div class="row action-buttons-<%=id%>">
                                         <div class="col-sm-6">
                                            <button uid="<%=id%>" type="button" class="skip btn-secondary btn btn-default">Discard</button>
                                         </div>
                                         <div class="col-sm-6">
                                            <button uid="<%=id%>" type="button" class="add btn-primary btn btn-default">Submit</button>
                                         </div>
                                     </div>
                                  </div>
                                </div>                                 
                          </div>`);

export class ExampleDocCov extends Component {
    displayName = ExampleDocCov.name

    constructor(props) {
        super(props);

        this.state = {};
        this.handleSelect();
    }

    handleSelect() {
      
                axios.get(`${Config.apiHost}/api/WebCrawler/GetExampleCovenants`)
                    .then(response => {
                        let fileContent = response.data;

                        axios.get(`${Config.apiHost}/api/WebCrawler/getCovenants`)
                            .then((response => {
                                this.state.covenants = response.data;

                                console.log(this.state);
                                this.setState({ ...this.state, fileContent: fileContent }, this.updateDocument);
                            }))
                    });
            
    }

    updateDocument() {
       
        let covenants = this.state.covenants;

        $(document).on('click', '.btn-ok', (event) => {
            this.add(event);
            event.stopPropagation();
            event.preventDefault();
            return false;
        });

        $(document).on('click', '.btn-remove', (event) => {
            this.skip(event);
            event.stopPropagation();
            event.preventDefault();
            return false;
        });

        for (let i in covenants) {

            let cov = covenants[i];
            let htmlFragment = covenantTemplate(cov);
            let elem = $(`[id='${cov.id}']`);
            elem.addClass('highlight');
            let panel = $(htmlFragment).insertBefore(elem);

            panel.on('click', (event) => {

if (event.target.tagName != 'DIV')
{
return;
}

                if ($(event.target).hasClass('btn-ok')) {
                    return;
                }

                if ($(event.target).hasClass('btn-remove')) {
                    return;
                }

                let collapsePanel = panel.children('.panel-collapse');

                if (!collapsePanel.hasClass('in')) {
                    collapsePanel.addClass('in');     
                    collapsePanel.parent().css('z-index', 1000000);
                } else {
                    collapsePanel.removeClass('in');
                    collapsePanel.parent().css('z-index', 1000);
                }

            }, );
        }

        $('.add').on('click', but => {
            this.add(but);
        });

        $('.skip').on('click', but => {
            this.skip(but);
        });
    }

    add(but) {

        let id = but.target.attributes['uid'].value;

        axios.post(`${Config.apiHost}/api/document/${this.state.documentId}/covenants/${id}/accept`)
            .then(x => {

                $(`.action-buttons-${id}`).replaceWith(`<div class="row"></div>`);
                $(`.head-remove-button-${id}`).remove();
                $(`[uid='${id}']`).addClass('panel-success');
                console.log(id);

            });
    }

    skip(but) {

        let id = but.target.attributes['uid'].value;

        axios.post(`${Config.apiHost}/api/document/${this.state.documentId}/covenants/${id}/reject`)
            .then(x => {

                let elem = $(`[id='${id}']`);
                elem.removeClass('highlight');
                let coven = $(`[uid='${id}']`);
                coven.remove();
                console.log(id);
                
            });
        
    }


    render() {
        return (
            <Panel className={'cov-list'}>
                <Panel.Heading>
                    Web Document
                </Panel.Heading>
                <Panel.Body className={'full-text'}>
                                <div dangerouslySetInnerHTML={{ __html: this.state.fileContent }}></div>
                            </Panel.Body>
            </Panel>
        );
    }
}
