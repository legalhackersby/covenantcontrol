import React, { Component } from 'react';
import axios from 'axios'
import { Col, Grid, Panel, Row, Form, Button } from 'react-bootstrap';
import './WebDocument.css'
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

export class WebDocument extends Component {
    displayName = WebDocument.name

    constructor(props) {
        super(props);

        this.state = {};
        this.handleSelect();
    }

    handleSelect() {
      
                axios.get(`${Config.apiHost}/api/WebCrawler/getJson`)
                    .then(response => {
                        let fileContent = response.data;

                        this.setState({ ...this.state, fileContent: fileContent }, this.updateDocument);
                    });
            
    }

    updateDocument() {
       
        
    }

    render() {
        return (
            <Panel className={'cov-list'}>
                <Panel.Heading>
                <Row>
                            <Col sm={2}>Web Document</Col>
                            <Col sm={6}> </Col>
                            <Col sm={2}>
                                <Link to={{ pathname: '/webDocumentCov/'}}>
                                    <button class="btn btn-primary">Web Document Cov</button>
                                </Link>
                            </Col>
                        </Row>
                </Panel.Heading>
                <Panel.Body className={'full-text'}>
                                <div dangerouslySetInnerHTML={{ __html: this.state.fileContent }}></div>
                            </Panel.Body>
            </Panel>
        );
    }
}
