import { useEffect, useRef, useState } from "react";
import api from "../services/api";

import "ol/ol.css";
import Map from "ol/Map";
import View from "ol/View";
import TileLayer from "ol/layer/Tile";
import VectorLayer from "ol/layer/Vector";
import OSM from "ol/source/OSM";
import VectorSource from "ol/source/Vector";
import Draw from "ol/interaction/Draw";
import Modify from "ol/interaction/Modify";
import Select from "ol/interaction/Select";
import WKT from "ol/format/WKT";
import { fromLonLat } from "ol/proj";
import { click } from "ol/events/condition";
import Style from "ol/style/Style";
import Stroke from "ol/style/Stroke";
import Fill from "ol/style/Fill";
import CircleStyle from "ol/style/Circle";

import { Button } from "primereact/button";
import { Message } from "primereact/message";
import { InputText } from "primereact/inputtext";
import { Dialog } from "primereact/dialog";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

function MapPage({ token, onLogout }) {
  const mapElement = useRef(null);
  const mapRef = useRef(null);
  const vectorSourceRef = useRef(null);
  const drawInteractionRef = useRef(null);
  const selectInteractionRef = useRef(null);

  const [drawType, setDrawType] = useState("");
  const [message, setMessage] = useState(null);
  const [loadedCount, setLoadedCount] = useState(0);
  const [selectedFeatureInfo, setSelectedFeatureInfo] = useState(null);

  const [geometries, setGeometries] = useState([]);

  const [saveDialogVisible, setSaveDialogVisible] = useState(false);
  const [pendingGeometry, setPendingGeometry] = useState(null);

  const [popupName, setPopupName] = useState("");
  const [popupColor, setPopupColor] = useState("#2563eb");
  const [popupDescription, setPopupDescription] = useState("");

  const [userInfo, setUserInfo] = useState({
    userId: null,
    email: "",
    fullName: "",
    role: "User",
  });

  function parseJwtToken() {
    try {
      if (!token) return null;

      const base64Url = token.split(".")[1];
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");

      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split("")
          .map((c) => {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join("")
      );

      return JSON.parse(jsonPayload);
    } catch {
      return null;
    }
  }

  function getUserInfoFromToken() {
    const decoded = parseJwtToken();

    if (!decoded) {
      const emptyUser = {
        userId: null,
        email: "",
        fullName: "",
        role: "User",
      };

      setUserInfo(emptyUser);
      return emptyUser;
    }

    const userId =
      decoded[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ] ||
      decoded.nameid ||
      decoded.userId ||
      null;

    const email =
      decoded[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
      ] ||
      decoded.email ||
      "";

    const fullName =
      decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ||
      decoded.unique_name ||
      decoded.fullName ||
      "";

    const role =
      decoded[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ] ||
      decoded.role ||
      "User";

    const parsedUser = {
      userId: userId ? Number(userId) : null,
      email: email,
      fullName: fullName,
      role: role,
    };

    setUserInfo(parsedUser);
    return parsedUser;
  }

  function getUserIdFromToken() {
    const decoded = parseJwtToken();

    if (!decoded) return null;

    const userId =
      decoded[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ] ||
      decoded.nameid ||
      decoded.userId ||
      null;

    return userId ? Number(userId) : null;
  }

  function getRoleFromToken() {
    const decoded = parseJwtToken();

    if (!decoded) return "User";

    return (
      decoded[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ] ||
      decoded.role ||
      "User"
    );
  }

  function isAdminUser() {
    return getRoleFromToken()?.toLowerCase() === "admin";
  }

  function normalizeGeometryType(geometryType) {
    if (!geometryType) return "";

    const type = geometryType.toString().toLowerCase();

    if (type === "point") return "Point";
    if (type === "linestring" || type === "line") return "LineString";
    if (type === "polygon") return "Polygon";

    return geometryType;
  }

  function getDefaultColor(type) {
    const normalizedType = normalizeGeometryType(type);

    if (normalizedType === "Point") return "#2563eb";
    if (normalizedType === "LineString") return "#16a34a";
    if (normalizedType === "Polygon") return "#dc2626";

    return "#2563eb";
  }

  function getIdFromOpenLayersFeatureId(featureId) {
    if (!featureId) return null;

    const raw = featureId.toString();

    if (raw.includes("-")) {
      const parts = raw.split("-");
      const lastPart = parts[parts.length - 1];

      return lastPart ? Number(lastPart) : null;
    }

    return Number(raw);
  }

  function hexToRgba(hex, alpha) {
    if (!hex || !hex.startsWith("#") || hex.length !== 7) {
      return `rgba(220, 38, 38, ${alpha})`;
    }

    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);

    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
  }

  function createFeatureStyle(color, geometryType) {
    const safeColor = color || getDefaultColor(geometryType);
    const normalizedType = normalizeGeometryType(geometryType);

    if (normalizedType === "Point") {
      return new Style({
        image: new CircleStyle({
          radius: 7,
          fill: new Fill({
            color: safeColor,
          }),
          stroke: new Stroke({
            color: "#ffffff",
            width: 2,
          }),
        }),
      });
    }

    if (normalizedType === "LineString") {
      return new Style({
        stroke: new Stroke({
          color: safeColor,
          width: 4,
        }),
      });
    }

    return new Style({
      stroke: new Stroke({
        color: safeColor,
        width: 3,
      }),
      fill: new Fill({
        color: hexToRgba(safeColor, 0.25),
      }),
    });
  }

  function removeCurrentDrawInteraction() {
    if (drawInteractionRef.current && mapRef.current) {
      mapRef.current.removeInteraction(drawInteractionRef.current);
      drawInteractionRef.current = null;
    }
  }

  async function loadExistingGeometries() {
    try {
      const currentUserId = getUserIdFromToken();
      const admin = isAdminUser();

      if (!currentUserId) {
        setMessage({
          severity: "warn",
          text: "Kullanıcı bilgisi alınamadı. Lütfen tekrar giriş yapın.",
        });
        return;
      }

      let response;

      if (admin) {
        response = await api.get("/DrawGeometry/all");
      } else {
        response = await api.get(`/DrawGeometry/user/${currentUserId}`);
      }

      const geometryList = (response.data || []).sort((a, b) => {
      const dateA = new Date(a.createdAt ?? a.CreatedAt ?? 0);
      const dateB = new Date(b.createdAt ?? b.CreatedAt ?? 0);

  return dateB - dateA;
});

      setGeometries(geometryList);

      if (!vectorSourceRef.current) return;

      vectorSourceRef.current.clear();

      if (selectInteractionRef.current) {
        selectInteractionRef.current.getFeatures().clear();
      }

      setSelectedFeatureInfo(null);

      const wktFormat = new WKT();

      geometryList.forEach((item) => {
        const itemId = item.id ?? item.Id;
        const itemName = item.name ?? item.Name ?? "İsimsiz çizim";
        const itemWkt = item.wkt ?? item.Wkt;
        const itemColor =
          item.color ?? item.Color ?? getDefaultColor(item.geometryType);
        const itemDescription = item.description ?? item.Description ?? "";
        const itemObjectCount = item.objectCount ?? item.ObjectCount ?? 0;

        const itemGeometryType = normalizeGeometryType(
          item.geometryType ?? item.GeometryType
        );

        const itemUserId = item.userId ?? item.UserId;

        if (!itemWkt) return;

        const feature = wktFormat.readFeature(itemWkt, {
          dataProjection: "EPSG:4326",
          featureProjection: "EPSG:3857",
        });

        feature.setId(`${itemGeometryType}-${itemId}`);

        feature.setProperties({
          dbId: itemId,
          id: itemId,
          Id: itemId,
          featureId: itemId,
          name: itemName,
          Name: itemName,
          color: itemColor,
          Color: itemColor,
          description: itemDescription,
          Description: itemDescription,
          objectCount: itemObjectCount,
          ObjectCount: itemObjectCount,
          geometryType: itemGeometryType,
          GeometryType: itemGeometryType,
          userId: itemUserId,
          UserId: itemUserId,
          wkt: itemWkt,
          Wkt: itemWkt,
        });

        feature.setStyle(createFeatureStyle(itemColor, itemGeometryType));

        vectorSourceRef.current.addFeature(feature);
      });

      setLoadedCount(geometryList.length);

      if (geometryList.length > 0) {
        setMessage({
          severity: "success",
          text: admin
            ? `${geometryList.length} kayıtlı çizim haritaya yüklendi. Admin görünümü: tüm kullanıcıların çizimleri listeleniyor.`
            : `${geometryList.length} kayıtlı çizim haritaya yüklendi.`,
        });
      } else {
        setMessage({
          severity: "info",
          text: admin
            ? "Sistemde kayıtlı çizim bulunamadı."
            : "Bu kullanıcıya ait kayıtlı çizim bulunamadı.",
        });
      }
    } catch (error) {
      setMessage({
        severity: "error",
        text:
          error.response?.data?.message || "Kayıtlı çizimler yüklenemedi.",
      });
    }
  }

  async function reloadGeometries() {
    if (vectorSourceRef.current) {
      vectorSourceRef.current.clear();
    }

    if (selectInteractionRef.current) {
      selectInteractionRef.current.getFeatures().clear();
    }

    setSelectedFeatureInfo(null);

    await loadExistingGeometries();
  }

  function startDrawing(type) {
    removeCurrentDrawInteraction();

    const defaultColor = getDefaultColor(type);

    setDrawType(type);
    setSelectedFeatureInfo(null);

    setMessage({
      severity: "info",
      text: `${type} çizim modu aktif. Haritada çizim yapabilirsiniz.`,
    });

    const drawInteraction = new Draw({
      source: vectorSourceRef.current,
      type: type,
    });

    drawInteraction.on("drawend", (event) => {
      const drawnFeature = event.feature;

      const wktFormat = new WKT();

      const wkt = wktFormat.writeFeature(drawnFeature, {
        dataProjection: "EPSG:4326",
        featureProjection: "EPSG:3857",
      });

      drawnFeature.setStyle(createFeatureStyle(defaultColor, type));

      removeCurrentDrawInteraction();
      setDrawType("");

      setPendingGeometry({
        type: type,
        wkt: wkt,
        feature: drawnFeature,
      });

      setPopupName(`${type} çizimi`);
      setPopupColor(defaultColor);
      setPopupDescription("");
      setSaveDialogVisible(true);
    });

    mapRef.current.addInteraction(drawInteraction);
    drawInteractionRef.current = drawInteraction;
  }

  async function savePendingGeometry() {
    try {
      if (!pendingGeometry) {
        setMessage({
          severity: "warn",
          text: "Kaydedilecek çizim bulunamadı.",
        });
        return;
      }

      const currentUserId = getUserIdFromToken();

      if (!currentUserId) {
        setMessage({
          severity: "warn",
          text: "Kullanıcı bilgisi bulunamadı. Lütfen tekrar giriş yapın.",
        });
        return;
      }

      const payload = {
        userId: currentUserId,
        name: popupName?.trim() || `${pendingGeometry.type} çizimi`,
        color: popupColor || getDefaultColor(pendingGeometry.type),
        description: popupDescription || "",
        geometryType: pendingGeometry.type,
        wkt: pendingGeometry.wkt,
      };

      const response = await api.post("/DrawGeometry/save", payload);

      if (response.data?.success) {
        setSaveDialogVisible(false);
        setPendingGeometry(null);

        await loadExistingGeometries();

        const savedObjectCount = response.data?.data?.objectCount ?? 0;

        if (normalizeGeometryType(payload.geometryType) === "Polygon") {
          setMessage({
            severity: "success",
            text: `Polygon başarıyla kaydedildi. Polygon altında kalan obje sayısı: ${savedObjectCount}`,
          });
        } else {
          setMessage({
            severity: "success",
            text: `${payload.geometryType} başarıyla veritabanına kaydedildi.`,
          });
        }
      }
    } catch (error) {
      setMessage({
        severity: "error",
        text:
          error.response?.data?.message ||
          "Geometri kaydedilirken hata oluştu.",
      });
    }
  }

  function cancelPendingGeometry() {
    if (pendingGeometry?.feature && vectorSourceRef.current) {
      vectorSourceRef.current.removeFeature(pendingGeometry.feature);
    }

    setSaveDialogVisible(false);
    setPendingGeometry(null);
    setPopupName("");
    setPopupDescription("");
    setPopupColor("#2563eb");

    setMessage({
      severity: "info",
      text: "Çizim kaydedilmeden iptal edildi.",
    });
  }

  async function softDeleteSelectedGeometry() {
    try {
      if (!selectedFeatureInfo) {
        setMessage({
          severity: "warn",
          text: "Lütfen önce haritadan bir çizim seçin.",
        });
        return;
      }

      const geometryType = selectedFeatureInfo.geometryType;
      const id = selectedFeatureInfo.id;

      if (!geometryType || !id) {
        setMessage({
          severity: "warn",
          text: "Seçili çizimin ID veya tip bilgisi alınamadı. Lütfen kayıtlı çizimleri yeniden yükleyip tekrar seçin.",
        });
        return;
      }

      const response = await api.delete(
        `/DrawGeometry/delete/${geometryType}/${id}`
      );

      if (response.data?.success) {
        if (selectedFeatureInfo.feature && vectorSourceRef.current) {
          vectorSourceRef.current.removeFeature(selectedFeatureInfo.feature);
        }

        if (selectInteractionRef.current) {
          selectInteractionRef.current.getFeatures().clear();
        }

        setSelectedFeatureInfo(null);
        await loadExistingGeometries();

        setMessage({
          severity: "success",
          text: "Seçili çizim başarıyla silindi.",
        });
      }
    } catch (error) {
      setMessage({
        severity: "error",
        text:
          error.response?.data?.message ||
          "Seçili çizim silinirken hata oluştu.",
      });
    }
  }

  function stopDrawing() {
    removeCurrentDrawInteraction();
    setDrawType("");

    setMessage({
      severity: "info",
      text: "Çizim modu durduruldu.",
    });
  }

  function clearMap() {
    if (vectorSourceRef.current) {
      vectorSourceRef.current.clear();
    }

    if (selectInteractionRef.current) {
      selectInteractionRef.current.getFeatures().clear();
    }

    setSelectedFeatureInfo(null);
    setLoadedCount(0);

    setMessage({
      severity: "info",
      text: "Harita temizlendi. Veritabanındaki kayıtlar silinmedi.",
    });
  }

  function findFeatureOnMap(rowData) {
    if (!vectorSourceRef.current) return null;

    const rowId = rowData.id ?? rowData.Id;
    const rowGeometryType = normalizeGeometryType(
      rowData.geometryType ?? rowData.GeometryType
    );

    const features = vectorSourceRef.current.getFeatures();

    return features.find((feature) => {
      const featureId =
        feature.get("dbId") ||
        feature.get("id") ||
        feature.get("Id") ||
        feature.get("featureId") ||
        getIdFromOpenLayersFeatureId(feature.getId());

      const featureGeometryType = normalizeGeometryType(
        feature.get("geometryType") || feature.get("GeometryType")
      );

      return (
        Number(featureId) === Number(rowId) &&
        featureGeometryType === rowGeometryType
      );
    });
  }

  function zoomToGeometry(rowData) {
    const feature = findFeatureOnMap(rowData);

    if (!feature || !mapRef.current) {
      setMessage({
        severity: "warn",
        text: "Haritada gösterilecek obje bulunamadı.",
      });
      return;
    }

    const geometry = feature.getGeometry();

    if (!geometry) {
      setMessage({
        severity: "warn",
        text: "Objenin geometrisi bulunamadı.",
      });
      return;
    }

    const view = mapRef.current.getView();
    const extent = geometry.getExtent();

    view.fit(extent, {
      padding: [80, 80, 80, 80],
      duration: 700,
      maxZoom: 17,
    });

    if (selectInteractionRef.current) {
      selectInteractionRef.current.getFeatures().clear();
      selectInteractionRef.current.getFeatures().push(feature);
    }

    setSelectedFeatureInfo({
      id: feature.get("dbId") || feature.get("id"),
      name: feature.get("name") || "İsimsiz çizim",
      color: feature.get("color"),
      description: feature.get("description"),
      objectCount: feature.get("objectCount") || 0,
      geometryType: normalizeGeometryType(feature.get("geometryType")),
      feature: feature,
    });

    setMessage({
      severity: "info",
      text: "Seçilen obje haritada gösterildi.",
    });
  }

  function actionBodyTemplate(rowData) {
    return (
      <Button
        label="Haritada Gör"
        icon="pi pi-search"
        size="small"
        severity="info"
        onClick={() => zoomToGeometry(rowData)}
      />
    );
  }

  function colorBodyTemplate(rowData) {
    const color = rowData.color ?? rowData.Color ?? "#2563eb";

    return (
      <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
        <span
          style={{
            width: "18px",
            height: "18px",
            borderRadius: "50%",
            backgroundColor: color,
            border: "1px solid #cbd5e1",
            display: "inline-block",
          }}
        ></span>
        <span>{color}</span>
      </div>
    );
  }

  function objectCountBodyTemplate(rowData) {
    const geometryType = normalizeGeometryType(
      rowData.geometryType ?? rowData.GeometryType
    );

    const count = rowData.objectCount ?? rowData.ObjectCount ?? 0;

    if (geometryType !== "Polygon") {
      return "-";
    }

    return count;
  }

  useEffect(() => {
    getUserInfoFromToken();

    const vectorSource = new VectorSource();
    vectorSourceRef.current = vectorSource;

    const baseLayer = new TileLayer({
      source: new OSM(),
    });

    const vectorLayer = new VectorLayer({
      source: vectorSource,
    });

    const map = new Map({
      target: mapElement.current,
      layers: [baseLayer, vectorLayer],
      view: new View({
        center: fromLonLat([35.2433, 38.9637]),
        zoom: 6,
      }),
    });

    const modify = new Modify({
      source: vectorSource,
    });

    map.addInteraction(modify);

    const selectInteraction = new Select({
      condition: click,
    });

    selectInteraction.on("select", (event) => {
      if (event.selected.length > 0) {
        const feature = event.selected[0];

        const geometry = feature.getGeometry();
        const geometryTypeFromMap = geometry ? geometry.getType() : "";

        const featureOpenLayersId = feature.getId();

        const selectedId =
          feature.get("dbId") ||
          feature.get("id") ||
          feature.get("Id") ||
          feature.get("featureId") ||
          getIdFromOpenLayersFeatureId(featureOpenLayersId);

        const selectedName =
          feature.get("name") || feature.get("Name") || "İsimsiz çizim";

        const selectedGeometryType =
          feature.get("geometryType") ||
          feature.get("GeometryType") ||
          geometryTypeFromMap;

        setSelectedFeatureInfo({
          id: selectedId,
          name: selectedName,
          color: feature.get("color"),
          description: feature.get("description"),
          objectCount: feature.get("objectCount") || 0,
          geometryType: normalizeGeometryType(selectedGeometryType),
          feature: feature,
        });

        setMessage({
          severity: "info",
          text: "Bir çizim seçildi.",
        });
      } else {
        setSelectedFeatureInfo(null);
      }
    });

    map.addInteraction(selectInteraction);

    mapRef.current = map;
    selectInteractionRef.current = selectInteraction;

    loadExistingGeometries();

    return () => {
      map.setTarget(undefined);
    };
  }, []);

  return (
    <div className="map-page">
      <div className="map-header">
        <div>
          <h2>Türkiye Harita Paneli</h2>
          <p>OpenLayers + OSM + PostGIS çizim ve sorgulama sistemi</p>
        </div>

        <div className="user-menu">
          <div className="user-info">
            <strong>{userInfo.fullName || "Kullanıcı"}</strong>
            <span>{userInfo.email}</span>
            <span>{userInfo.role}</span>
          </div>

          <Button
            label="Çıkış Yap"
            icon="pi pi-sign-out"
            severity="danger"
            outlined
            onClick={onLogout}
          />
        </div>
      </div>

      <div className="map-content">
        <div className="tools-panel">
          <h3>Çizim Araçları</h3>

          <div className="draw-count">
            Kayıtlı çizim sayısı: <strong>{loadedCount}</strong>
          </div>

          {userInfo.role?.toLowerCase() === "admin" && (
            <div className="draw-count">
              <strong>Admin görünümü:</strong> Tüm kullanıcıların çizimleri
              listelenir.
            </div>
          )}

          <Button
            label="Point Çiz"
            icon="pi pi-map-marker"
            onClick={() => startDrawing("Point")}
            className={drawType === "Point" ? "tool-active" : ""}
          />

          <Button
            label="Line Çiz"
            icon="pi pi-minus"
            onClick={() => startDrawing("LineString")}
            className={drawType === "LineString" ? "tool-active" : ""}
          />

          <Button
            label="Polygon Çiz"
            icon="pi pi-stop"
            onClick={() => startDrawing("Polygon")}
            className={drawType === "Polygon" ? "tool-active" : ""}
          />

          <Button
            label="Çizimi Durdur"
            icon="pi pi-pause"
            severity="secondary"
            outlined
            onClick={stopDrawing}
          />

          <Button
            label="Haritayı Temizle"
            icon="pi pi-trash"
            severity="warning"
            outlined
            onClick={clearMap}
          />

          <Button
            label="Kayıtlı Çizimleri Yükle"
            icon="pi pi-refresh"
            severity="help"
            outlined
            onClick={reloadGeometries}
          />

          {selectedFeatureInfo && (
            <div className="selected-box">
              <h4>Seçili Çizim</h4>

              <p>
                <strong>Ad:</strong> {selectedFeatureInfo.name}
              </p>

              <p>
                <strong>Tip:</strong> {selectedFeatureInfo.geometryType}
              </p>

              <p>
                <strong>ID:</strong> {selectedFeatureInfo.id}
              </p>

              <p>
                <strong>Renk:</strong> {selectedFeatureInfo.color || "-"}
              </p>

              <p>
                <strong>Açıklama:</strong>{" "}
                {selectedFeatureInfo.description || "-"}
              </p>

              {selectedFeatureInfo.geometryType === "Polygon" && (
                <p>
                  <strong>Altındaki obje sayısı:</strong>{" "}
                  {selectedFeatureInfo.objectCount ?? 0}
                </p>
              )}

              <Button
                label="Seçili Çizimi Sil"
                icon="pi pi-times"
                severity="danger"
                onClick={softDeleteSelectedGeometry}
              />
            </div>
          )}

          {message && (
            <Message
              severity={message.severity}
              text={message.text}
              className="map-message"
            />
          )}
        </div>

        <div className="main-map-area">
          <div className="map-wrapper">
            <div ref={mapElement} className="map"></div>
          </div>

          <div className="query-panel">
            <div className="query-panel-header">
              <div>
                <h3>Sorgulama Ekranı</h3>
                <p>Veritabanındaki kayıtlı çizimler</p>
              </div>

              <Button
                label="Yenile"
                icon="pi pi-refresh"
                size="small"
                outlined
                onClick={reloadGeometries}
              />
            </div>

            <DataTable
              value={geometries}
              paginator
              rows={5}
              size="small"
              emptyMessage="Kayıtlı çizim bulunamadı."
              className="geometry-table"
            >
              <Column field="id" header="ID" style={{ width: "70px" }} />

              {userInfo.role?.toLowerCase() === "admin" && (
                <Column field="userId" header="User ID" />
              )}

              <Column field="name" header="İsim" />
              <Column field="geometryType" header="Tip" />
              <Column header="Renk" body={colorBodyTemplate} />
              <Column field="description" header="Açıklama" />
              <Column header="Obje Sayısı" body={objectCountBodyTemplate} />
              <Column header="İşlem" body={actionBodyTemplate} />
            </DataTable>
          </div>
        </div>
      </div>

      <Dialog
        header="Çizim Bilgilerini Kaydet"
        visible={saveDialogVisible}
        style={{ width: "420px" }}
        modal
        onHide={cancelPendingGeometry}
        footer={
          <div
            style={{ display: "flex", justifyContent: "flex-end", gap: "8px" }}
          >
            <Button
              label="İptal"
              icon="pi pi-times"
              severity="secondary"
              outlined
              onClick={cancelPendingGeometry}
            />
            <Button
              label="Kaydet"
              icon="pi pi-save"
              severity="success"
              onClick={savePendingGeometry}
            />
          </div>
        }
      >
        <div className="popup-form">
          <div className="field">
            <label>İsim</label>
            <InputText
              value={popupName}
              onChange={(e) => setPopupName(e.target.value)}
              placeholder="Örn: Riskli bölge"
            />
          </div>

          <div className="field">
            <label>Renk</label>
            <input
              type="color"
              value={popupColor}
              onChange={(e) => setPopupColor(e.target.value)}
              style={{
                width: "100%",
                height: "42px",
                border: "1px solid #cbd5e1",
                borderRadius: "8px",
                padding: "4px",
              }}
            />
          </div>

          <div className="field">
            <label>Açıklama</label>
            <InputText
              value={popupDescription}
              onChange={(e) => setPopupDescription(e.target.value)}
              placeholder="Bu çizim hakkında açıklama girin"
            />
          </div>

          {pendingGeometry?.type === "Polygon" && (
            <Message
              severity="info"
              text="Polygon kaydedilince altında kalan objelerin sayısı otomatik hesaplanacaktır."
            />
          )}
        </div>
      </Dialog>
    </div>
  );
}

export default MapPage;