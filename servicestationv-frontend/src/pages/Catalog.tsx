import React from 'react';
import '../styles/Catalog.css';
import Header  from '../components/Header';
import Main from '../components/Main';
import Footer from '../components/Footer'

function App() {
  return (
    <div className="App">
      <div className='background-overlay'/>
      <Header />
        <Main />
      <Footer />
    </div>
  );
}

export default App;
